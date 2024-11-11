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

    // ������ �� ���������� ������ ������
    public Transform ballVisual;

    // ������ �� ������ ��� ��������
    public Transform ballRotation;

    // ����� ���������� ��� �������� ������ �� ������
    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (ballRotation == null || ballVisual == null)
        {
            Debug.LogError("���������� ��������� ballRotation � ballVisual � ����������.");
            return;
        }

        animator = ballVisual.GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator �� ������ �� ������� " + ballVisual.gameObject.name);
        }
        else
        {
            animator.applyRootMotion = false;
        }

        // ���� ������ �� ���������, ���������� �������� ������
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // �������� ���� �� ������������
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // ��������� �������� �������� � ���������
        float speed = new Vector3(horizontalInput, 0, verticalInput).magnitude;
        animator.SetFloat("Speed", speed);

        // �������� �� ������
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // �������� ������ � ������ ���������� ������
        Vector3 movement = Vector3.zero;

        // �������� ����������� ������
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f; // ���������� ������������ ������������
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f; // ���������� ������������ ������������
        right.Normalize();

        // ������������ ��������
        movement = (forward * verticalInput + right * horizontalInput).normalized * moveSpeed;

        // �������� ������ � ������� ����
        rb.AddForce(movement);

        // �������� ������� ballRotation
        RotateVisualBall(movement);

        Debug.Log("�������� ������: " + rb.velocity.magnitude);
    }

    void LateUpdate()
    {
        if (isJumpingAnimation)
        {
            // ��������� �������� `BallVisual` ���������
            ballVisual.rotation = Quaternion.identity;
        }
        else
        {
            // ��������� `BallVisual` ��������� ������ � `BallRotation`, �� ��������� ��� Y
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
        // ���������, ��� ����� ������������� � �����
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
}
