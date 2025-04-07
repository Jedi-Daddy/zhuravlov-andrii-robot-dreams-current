using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int score = 0;
    public Text scoreText;

    public GameObject victoryPanel; // UI ������ ������
    public Button menuButton;       // ������ �������� � ����
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

        // �������� ������ ������
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // ���������� ����
        Time.timeScale = 0f;

        // �������� ������
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Victory!");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // ������� ����� ����� ��������� �����
        SceneManager.LoadScene("MenuLection16"); // �������� �� ��� ����� ����� ����
    }
}
