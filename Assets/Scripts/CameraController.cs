using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // ����, �� ������� ����� ��������� ������ (�����)
    public Vector3 offset = new Vector3(0, 5, -10); // ��������� �������� ������
    public float smoothSpeed = 0.125f; // �������� ����������� ����������� ������

    public float rotationSpeed = 5f; // �������� �������� ������
    private float currentYaw = 0f; // ������� ���� �������� �� ��� Y

    public float zoomSpeed = 2f; // �������� ����
    public float minZoom = 5f; // ����������� ���������� ������ �� ����
    public float maxZoom = 15f; // ������������ ���������� ������ �� ����
    private float currentZoom = 10f; // ������� ���������� ������ �� ����

    public LayerMask obstructionMask; // ����(�) �����������, ������� ����� ����������� ������

    void Start()
    {
        // ������������� ������� ���������� ������ �� ������ ���������� ��������
        currentZoom = offset.magnitude;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        // ��������� ���� � ������� �������� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // �������� ������ ��� ����������� ������ ������ ����
        if (Input.GetMouseButton(1)) // ������ ������ ���� ������������
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            currentYaw += mouseInputX * rotationSpeed;
        }

        // ������������ �������� ������� ������ � ������ �������� � ����
        Quaternion rotation = Quaternion.Euler(0, currentYaw, 0);
        Vector3 desiredPosition = target.position + rotation * (offset.normalized * currentZoom);

        // ��������� ������� ����������� ����� ����� � �������� �������� ������
        RaycastHit hit;
        Vector3 direction = desiredPosition - target.position;
        float distance = currentZoom;

        if (Physics.Raycast(target.position, direction.normalized, out hit, currentZoom, obstructionMask))
        {
            // ���� ���������� �����������, ������������ ������� ������ ����� ������������
            distance = hit.distance - 0.5f; // ��������� ������� �� �����������
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
            desiredPosition = target.position + rotation * (offset.normalized * distance);
        }

        // ������� ����������� ������ � �������� �������
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ������������� ������� ������
        transform.LookAt(target.position);
    }
}
