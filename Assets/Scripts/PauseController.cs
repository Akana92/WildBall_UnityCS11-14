using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;

    void Update()
    {
        // Проверяем нажатие клавиши P
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Если игра на паузе, снимаем с паузы
            }
            else
            {
                PauseGame(); // Если игра не на паузе, ставим на паузу
            }
        }
    }

    // Метод для установки игры на паузу
    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Останавливаем время
    }

    // Метод для возобновления игры
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Возобновляем время
    }

    // Метод, вызываемый при нажатии кнопки "Перезапустить"
    public void OnRestartButtonPressed()
    {
        Time.timeScale = 1f; // Возобновляем время перед перезапуском
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Метод, вызываемый при нажатии кнопки "Выход в меню"
    public void OnExitButtonPressed()
    {
        Time.timeScale = 1f; // Возобновляем время перед выходом
        SceneManager.LoadScene(0); // Предполагаем, что главная сцена имеет индекс 0
    }
}
