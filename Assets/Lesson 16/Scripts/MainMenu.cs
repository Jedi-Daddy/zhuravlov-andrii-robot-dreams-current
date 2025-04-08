using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Lection20"); 
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Чтоб выход работал в редакторе
#endif
    }
    // Сброс состояния игры
    private void ResetGameState()
    {
        // Пример сброса всех глобальных переменных
        // Например: Score = 0;
        // Если используешь какие-то данные в PlayerPrefs — очищай их
        PlayerPrefs.DeleteAll();
    }

    public void OnStartGame()
    {
        // Сбрасываем состояние игры перед началом
        ResetGameState();
        // Перезагружаем сцену игры (например, "GameScene")
        SceneManager.LoadScene("Lection20");
    }
}
