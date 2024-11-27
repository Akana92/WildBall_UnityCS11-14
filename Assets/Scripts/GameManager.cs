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
    public GameObject gameWonPanel; // Новая панель для победы в игре

    // Эффект завершения уровня
    public ParticleSystem levelCompleteEffect;

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
        if (gameWonPanel != null)
            gameWonPanel.SetActive(false);

        // Убеждаемся, что эффект завершения уровня отключен при старте
        if (levelCompleteEffect != null)
        {
            levelCompleteEffect.Stop();
        }
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
        //Time.timeScale = 0f;
        isPaused = true;

        // Воспроизводим эффект завершения уровня
        if (levelCompleteEffect != null)
        {
            levelCompleteEffect.Play();
        }

        // Проверяем, является ли текущая сцена последней
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 6) // Замените 5 на индекс вашей последней сцены
        {
            // Это последний уровень, показываем панель победы
            if (gameWonPanel != null)
            {
                gameWonPanel.SetActive(true);
            }
        }
        else
        {
            // Не последний уровень, показываем панель завершения уровня
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);
            }
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
            // Если это последний уровень, перенаправляем на первую игровую сцену
            LoadFirstLevel();
        }
    }

    // Метод для загрузки первой игровой сцены
    public void LoadFirstLevel()
    {
        Time.timeScale = 1f; // Возобновляем время
        isPaused = false;

        // Замените 1 на индекс вашей первой игровой сцены
        SceneManager.LoadScene(2);
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
