using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Синглтон для доступа к GameManager из других скриптов
    public static GameManager instance;

    // Панели
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;

    private bool isPaused = false;

    void Awake()
    {
        // Настраиваем синглтон
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Отключаем панели при старте
        if (pausePanel != null)
            pausePanel.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    void Update()
    {
        // Проверяем нажатие клавиши Esc для паузы
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
        if (pausePanel != null)
            pausePanel.SetActive(true);
        Time.timeScale = 0f; // Останавливаем время
    }

    // Метод для возобновления игры
    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f; // Возобновляем время
    }

    // Метод вызывается при проигрыше
    public void GameOver()
    {
        // Останавливаем время
        Time.timeScale = 0f;
        isPaused = true;

        // Показываем панель "Вы проиграли"
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Метод вызывается при завершении уровня
    public void LevelComplete()
    {
        // Останавливаем время
        Time.timeScale = 0f;
        isPaused = true;

        // Показываем панель "Уровень пройден"
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
    }

    // Метод для перезапуска текущего уровня
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Возобновляем время
        isPaused = false;

        // Перезапускаем текущий уровень
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Метод для загрузки следующего уровня
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Возобновляем время
        isPaused = false;

        // Получаем индекс текущей сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Проверяем, существует ли следующий уровень
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            // Загружаем следующий уровень
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            // Если это последний уровень, можно перейти в главное меню или показать сообщение
            Debug.Log("Все уровни пройдены!");
        }
    }

    // Метод, вызываемый при нажатии кнопки "Выход в меню"
    public void OnExitButtonPressed()
    {
        Time.timeScale = 1f; // Возобновляем время перед выходом
        isPaused = false;

        // Переходим на сцену с индексом 0 (главное меню)
        SceneManager.LoadScene(0);
    }
}
