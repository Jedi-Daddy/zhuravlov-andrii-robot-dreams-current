using UnityEngine;
using UnityEngine.InputSystem; // Нужно подключить для работы с новым Input System
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private PlayerInput playerInput; // Для обработки ввода через InputSystem

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // Получаем доступ к компоненту PlayerInput
    }

    void OnEnable()
    {
        // Подключаем метод для обработки нажатия клавиши Escape
        playerInput.actions["Escape"].started += OnEscapePressed; 
    }

    void OnDisable()
    {
        // Отсоединяем обработчик, чтобы избежать утечек памяти
        playerInput.actions["Escape"].started -= OnEscapePressed;
    }

    private void ResetGameState()
    {
        // Пример сброса всех глобальных переменных
        // Например: Score = 0;
        // Если используешь какие-то данные в PlayerPrefs — очищай их
        PlayerPrefs.DeleteAll();
    }


    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        // Когда нажата клавиша Escape, загружаем главную сцену
        SceneManager.LoadScene("MenuLection16");
        ResetGameState(); // Сбрасываем состояние игры
    }

}
