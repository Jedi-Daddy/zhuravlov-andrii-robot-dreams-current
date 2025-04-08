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
        UnityEditor.EditorApplication.isPlaying = false; // ���� ����� ������� � ���������
#endif
    }
    // ����� ��������� ����
    private void ResetGameState()
    {
        // ������ ������ ���� ���������� ����������
        // ��������: Score = 0;
        // ���� ����������� �����-�� ������ � PlayerPrefs � ������ ��
        PlayerPrefs.DeleteAll();
    }

    public void OnStartGame()
    {
        // ���������� ��������� ���� ����� �������
        ResetGameState();
        // ������������� ����� ���� (��������, "GameScene")
        SceneManager.LoadScene("Lection20");
    }
}
