using UnityEngine;

public class Trap : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Вызываем метод проигрыша игры
            GameManager.instance.GameOver();
        }
    }
}
