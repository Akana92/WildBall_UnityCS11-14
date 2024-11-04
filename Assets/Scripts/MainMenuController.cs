using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject levelSelectPanel; // Панель с кнопками выбора уровней

    // Метод, вызываемый при нажатии кнопки Play
    public void OnPlayButtonPressed()
    {
        // Показываем панель выбора уровней
        levelSelectPanel.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        // Скрываем панель выбора уровней
        levelSelectPanel.SetActive(false);
    }

    public void LoadLevel(int levelIndex)
    {
        // Загружаем сцену с указанным индексом
        SceneManager.LoadScene(levelIndex);
    }
}
