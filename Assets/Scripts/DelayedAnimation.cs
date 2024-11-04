using UnityEngine;
using System.Collections;

public class DelayedAnimation : MonoBehaviour
{
    [Tooltip("Задержка перед началом анимации в секундах")]
    public float delay = 0f; // Настраиваемая задержка

    [Tooltip("Название состояния анимации для воспроизведения")]
    public string animationStateName = "MyAnimation"; // Название анимации

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Компонент Animator не найден на объекте " + gameObject.name);
            return;
        }

        // Останавливаем анимацию, установив скорость в 0
        animator.speed = 0f;

        // Запускаем корутину для начала анимации после задержки
        StartCoroutine(StartAnimationAfterDelay());
    }

    IEnumerator StartAnimationAfterDelay()
    {
        // Ждём заданную задержку
        yield return new WaitForSeconds(delay);

        // Запускаем конкретное состояние анимации
        animator.Play(animationStateName);

        // Запускаем анимацию, установив скорость в 1
        animator.speed = 1f;
    }
}
