using UnityEngine;

public class Trap : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Получаем ссылку на скрипт BallController
            BallController playerController = other.GetComponent<BallController>();
            if (playerController != null)
            {
                // Вызываем метод Die() у игрока
                playerController.Die();
            }
        }
    }
}
