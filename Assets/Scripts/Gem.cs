using UnityEngine;

public class Gem : MonoBehaviour
{
    private Animator animator;
    private bool isCollected = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator не найден на объекте " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            // Запускаем анимацию исчезновения
            animator.SetTrigger("Collected");
            isCollected = true;

            // Отключаем коллайдер, чтобы предотвратить повторные столкновения
            GetComponent<Collider>().enabled = false;

            // Запускаем корутину для уничтожения объекта после анимации
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // Ждём длительность анимации исчезновения
        float disappearAnimationLength = 1.0f; // Укажите реальную длительность
        yield return new WaitForSeconds(disappearAnimationLength);

        // Уничтожаем родительский объект вместе с дочерним
        Destroy(transform.parent.gameObject);
    }
}
