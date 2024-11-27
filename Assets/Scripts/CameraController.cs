using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Цель, за которой будет следовать камера (шарик)
    public Vector3 offset = new Vector3(0, 5, -10); // Начальное смещение камеры
    public float smoothSpeed = 0.125f; // Скорость сглаживания перемещения камеры

    public float rotationSpeed = 5f; // Скорость вращения камеры
    private float currentYaw = 0f; // Текущий угол поворота по оси Y (горизонталь)
    private float currentPitch = 20f; // Текущий угол поворота по оси X (вертикаль)

    public float minPitch = -30f; // Минимальный угол поворота по оси X
    public float maxPitch = 60f; // Максимальный угол поворота по оси X

    public float zoomSpeed = 2f; // Скорость зума
    public float minZoom = 5f; // Минимальное расстояние камеры до цели
    public float maxZoom = 15f; // Максимальное расстояние камеры до цели
    private float currentZoom = 10f; // Текущее расстояние камеры до цели

    public LayerMask obstructionMask; // Слой(и) препятствий, которые могут блокировать камеру

    void Start()
    {
        // Устанавливаем текущее расстояние камеры на основе начального смещения
        currentZoom = offset.magnitude;

        // Инициализируем начальные углы поворота на основе начального смещения
        Vector3 direction = offset.normalized;
        currentPitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        currentYaw = Mathf.Atan2(direction.x, -direction.z) * Mathf.Rad2Deg;
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
            float mouseInputY = Input.GetAxis("Mouse Y");

            currentYaw += mouseInputX * rotationSpeed;
            currentPitch -= mouseInputY * rotationSpeed; // Инвертируем управление по вертикали, при необходимости можно убрать минус

            // Ограничиваем угол поворота по вертикали
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }

        // Рассчитываем желаемую позицию камеры с учётом поворота и зума
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = target.position + rotation * new Vector3(0, 0, -currentZoom);

        // Проверяем наличие препятствий между целью и желаемой позицией камеры
        RaycastHit hit;
        Vector3 directionToCamera = desiredPosition - target.position;
        float distance = currentZoom;

        if (Physics.Raycast(target.position, directionToCamera.normalized, out hit, currentZoom, obstructionMask))
        {
            // Если обнаружено препятствие, корректируем позицию камеры перед препятствием
            distance = hit.distance - 0.5f; // Отступаем немного от препятствия
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
            desiredPosition = target.position + rotation * new Vector3(0, 0, -distance);
        }

        // Плавное перемещение камеры к желаемой позиции
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Устанавливаем поворот камеры, чтобы она смотрела на цель
        transform.LookAt(target.position);
    }
}
