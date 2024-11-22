using UnityEngine;

public class BallController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Animator animator;

    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded = true;
    private bool isJumpingAnimation = false;

    // Ссылка на визуальный объект шарика
    public Transform ballVisual;

    // Ссылка на объект для вращения
    public Transform ballRotation;

    // Ссылка на камеру
    public Transform cameraTransform;

    // Новые переменные для смерти
    public ParticleSystem deathEffect; // Система частиц смерти
    private bool isDead = false; // Флаг смерти

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (ballRotation == null || ballVisual == null)
        {
            Debug.LogError("Необходимо назначить ballRotation и ballVisual в инспекторе.");
            return;
        }

        animator = ballVisual.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator не найден на объекте " + ballVisual.gameObject.name);
        }
        else
        {
            animator.applyRootMotion = false;
        }

        // Если камера не назначена, используем основную камеру
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Отключаем систему частиц смерти при старте
        if (deathEffect != null)
        {
            deathEffect.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Система частиц смерти не назначена в инспекторе.");
        }
    }

    void Update()
    {
        if (isDead)
            return;

        // Получаем ввод от пользователя
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Обновляем параметр скорости в аниматоре
        float speed = new Vector3(horizontalInput, 0, verticalInput).magnitude;
        animator.SetFloat("Speed", speed);

        // Проверка на прыжок
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        // Движение шарика с учётом ориентации камеры
        Vector3 movement = Vector3.zero;

        // Получаем направления камеры
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f; // Игнорируем вертикальную составляющую
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f; // Игнорируем вертикальную составляющую
        right.Normalize();

        // Рассчитываем движение
        movement = (forward * verticalInput + right * horizontalInput).normalized * moveSpeed;

        // Движение шарика с помощью силы
        rb.AddForce(movement);

        // Вращение объекта ballRotation
        RotateVisualBall(movement);
    }

    void LateUpdate()
    {
        if (isDead)
            return;

        if (isJumpingAnimation)
        {
            // Фиксируем вращение `BallVisual` полностью
            ballVisual.rotation = Quaternion.identity;
        }
        else
        {
            // Позволяем `BallVisual` вращаться вместе с `BallRotation`, но фиксируем ось Y
            Vector3 eulerAngles = ballVisual.localEulerAngles;
            eulerAngles.y = 0;
            ballVisual.localEulerAngles = eulerAngles;
        }
    }

    void RotateVisualBall(Vector3 movement)
    {
        if (movement != Vector3.zero && !isJumpingAnimation)
        {
            float rotationSpeed = moveSpeed * 10f;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement.normalized);
            ballRotation.Rotate(rotationAxis, rotationSpeed * Time.fixedDeltaTime, Space.World);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("IsJumping", true);
        isGrounded = false;
        isJumpingAnimation = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead)
            return;

        // Проверяем, что шарик соприкоснулся с землёй
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                animator.SetBool("IsJumping", false);
                isJumpingAnimation = false;
            }
        }
    }

    // Новый метод для обработки смерти игрока
    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        // Отключаем визуализацию игрока
        ballVisual.gameObject.SetActive(false);
        ballRotation.gameObject.SetActive(false);

        // Останавливаем движение
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // Воспроизводим эффект смерти
        if (deathEffect != null)
        {
            deathEffect.gameObject.SetActive(true);
            deathEffect.Play();
        }
        else
        {
            Debug.LogWarning("Система частиц смерти не назначена.");
        }

        // Вызываем метод GameOver через GameManager
        GameManager.instance.GameOver();
    }
}
