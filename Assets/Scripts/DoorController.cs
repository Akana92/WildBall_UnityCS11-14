using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator; // Ссылка на компонент Animator
    private bool isPlayerNearby = false; // Флаг, указывающий, что игрок рядом
    private bool isOpen = false; // Состояние двери (открыта/закрыта)

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator не найден на двери!");
            }
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen; // Переключаем состояние двери

        // Устанавливаем параметр в Animator
        if (animator != null)
        {
            animator.SetBool("isOpen", isOpen);
        }

        // Можно добавить звук открытия/закрытия двери
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Можно показать подсказку "Нажмите E, чтобы открыть дверь"
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            // Скрыть подсказку
        }
    }
}
