using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Цель, за которой будет следовать камера (шарик)
    public Vector3 offset = new Vector3(0, 5, -10); // Начальное смещение камеры
    public float smoothSpeed = 0.125f; // Скорость сглаживания перемещения камеры

    public float rotationSpeed = 5f; // Скорость вращения камеры
    private float currentYaw = 0f; // Текущий угол поворота по оси Y

    public float zoomSpeed = 2f; // Скорость зума
    public float minZoom = 5f; // Минимальное расстояние камеры до цели
    public float maxZoom = 15f; // Максимальное расстояние камеры до цели
    private float currentZoom = 10f; // Текущее расстояние камеры до цели

    public LayerMask obstructionMask; // Слой(и) препятствий, которые могут блокировать камеру

    void Start()
    {
        // Устанавливаем текущее расстояние камеры на основе начального смещения
        currentZoom = offset.magnitude;
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        // Обработка зума с помощью колесика мыши
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Вращение камеры при удерживании правой кнопки мыши
        if (Input.GetMouseButton(1)) // Правая кнопка мыши удерживается
        {
            float mouseInputX = Input.GetAxis("Mouse X");
            currentYaw += mouseInputX * rotationSpeed;
        }

        // Рассчитываем желаемую позицию камеры с учётом поворота и зума
        Quaternion rotation = Quaternion.Euler(0, currentYaw, 0);
        Vector3 desiredPosition = target.position + rotation * (offset.normalized * currentZoom);

        // Проверяем наличие препятствий между целью и желаемой позицией камеры
        RaycastHit hit;
        Vector3 direction = desiredPosition - target.position;
        float distance = currentZoom;

        if (Physics.Raycast(target.position, direction.normalized, out hit, currentZoom, obstructionMask))
        {
            // Если обнаружено препятствие, корректируем позицию камеры перед препятствием
            distance = hit.distance - 0.5f; // Отступаем немного от препятствия
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
            desiredPosition = target.position + rotation * (offset.normalized * distance);
        }

        // Плавное перемещение камеры к желаемой позиции
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Устанавливаем поворот камеры
        transform.LookAt(target.position);
    }
}
