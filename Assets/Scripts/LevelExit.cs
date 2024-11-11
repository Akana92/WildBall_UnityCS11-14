using UnityEngine;

public class LevelExit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Вызываем метод GameOver из GameManager
            GameManager.instance.LevelComplete();
        }
    }
}
