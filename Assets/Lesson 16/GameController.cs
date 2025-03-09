using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the main scene (menu)
            SceneManager.LoadScene("MenuLection16"); 
        }
    }
    public void ResetGameState()
    {
        // Сброс всех глобальных переменных
        // Например: Score = 0;
        // Если используешь какие-то данные в PlayerPrefs — очищай их
        PlayerPrefs.DeleteAll();
    }
}
