using UnityEngine;

public class Gem : MonoBehaviour
{
    private Animator animator;
    private bool isCollected = false;

    public ParticleSystem pickupEffect; // Ссылка на систему частиц
    public AudioClip pickupSound; // (Опционально) Звук при подборе

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator не найден на объекте " + gameObject.name);
        }

        if (pickupEffect == null)
        {
            // Находим систему частиц среди дочерних объектов
            pickupEffect = GetComponentInChildren<ParticleSystem>(true); // Включаем поиск в неактивных объектах
            if (pickupEffect == null)
            {
                Debug.LogWarning("Система частиц не найдена на объекте " + gameObject.name);
            }
        }

        // Отключаем систему частиц при старте
        if (pickupEffect != null)
        {
            pickupEffect.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;

            // Воспроизводим эффект частиц
            if (pickupEffect != null)
            {
                // Отсоединяем эффект от бонуса, чтобы он не уничтожился вместе с ним
                pickupEffect.transform.parent = null;

                // Устанавливаем позицию эффекта на позицию гема
                pickupEffect.transform.position = transform.position;

                // Активируем систему частиц
                pickupEffect.gameObject.SetActive(true);

                // Воспроизводим эффект
                pickupEffect.Play();

                // Уничтожаем систему частиц после завершения воспроизведения
                Destroy(pickupEffect.gameObject, pickupEffect.main.duration + pickupEffect.main.startLifetime.constantMax);
            }

            // Воспроизводим звук при подборе
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Запускаем анимацию исчезновения
            if (animator != null)
            {
                animator.SetTrigger("Collected");
            }

            // Отключаем коллайдер, чтобы предотвратить повторные столкновения
            GetComponent<Collider>().enabled = false;

            // Запускаем корутину для уничтожения объекта после анимации
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // Ждём длительность анимации исчезновения
        float disappearAnimationLength = 1.0f; // Укажите реальную длительность анимации
        yield return new WaitForSeconds(disappearAnimationLength);

        // Уничтожаем объект бонуса
        Destroy(gameObject);
    }
}
