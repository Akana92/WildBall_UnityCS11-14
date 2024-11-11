using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Animator animator; // Ссылка на компонент Animator
    public GameObject trapToDisable; // Ловушка, которую нужно отключить
    public GameObject hintCanvas; // Ссылка на Canvas с подсказкой

    private bool isPlayerNearby = false; // Флаг, указывающий, что игрок рядом
    private bool isActivated = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator не найден на кнопке!");
            }
        }

        // Убеждаемся, что подсказка скрыта при старте
        if (hintCanvas != null)
        {
            hintCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        if (isActivated) return;

        isActivated = true;

        // Воспроизводим анимацию нажатия
        if (animator != null)
        {
            animator.SetTrigger("Pressed");
        }

        // Отключаем ловушку
        if (trapToDisable != null)
        {
            trapToDisable.SetActive(false);
        }

        // Скрываем подсказку после нажатия кнопки
        if (hintCanvas != null)
        {
            hintCanvas.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Показываем подсказку
            if (hintCanvas != null)
            {
                hintCanvas.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            // Скрываем подсказку
            if (hintCanvas != null)
            {
                hintCanvas.SetActive(false);
            }
        }
    }
}
