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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator не найден на объекте " + gameObject.name);
        }
        else
        {
            animator.applyRootMotion = false;
        }
    }

    void Update()
    {
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
        // Движение шарика с помощью силы
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        rb.AddForce(movement * moveSpeed);

        // Вращение шарика для имитации катящегося движения
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement);
        rb.AddTorque(rotationAxis * moveSpeed);

        Debug.Log("Скорость шарика: " + rb.velocity.magnitude);
    }


    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("IsJumping", true);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded && collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                animator.SetBool("IsJumping", false);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (isGrounded && collision.contacts.Length == 0)
        {
            isGrounded = false;
            animator.SetBool("IsJumping", true);
        }
    }
}
