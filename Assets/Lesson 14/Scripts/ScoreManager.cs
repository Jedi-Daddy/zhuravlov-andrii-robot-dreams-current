using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int score = 0;
    public Text scoreText;

    public GameObject victoryPanel; // UI панель победы
    public Button menuButton;       // Кнопка возврата в меню
    public int scoreToWin = 300;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (menuButton != null)
            menuButton.onClick.AddListener(ReturnToMenu);
    }

    public void AddScore(int amount)
    {
        if (gameEnded) return;

        score += amount;
        UpdateScoreText();

        if (score >= scoreToWin)
        {
            WinGame();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("scoreText not assigned to ScoreManager!");
        }
    }

    private void WinGame()
    {
        gameEnded = true;

        // Показать панель победы
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // Остановить игру
        Time.timeScale = 0f;

        // Показать курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Victory!");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Вернуть время перед загрузкой сцены
        SceneManager.LoadScene("MenuLection16"); // Заменить на имя вашей сцены меню
    }
}
