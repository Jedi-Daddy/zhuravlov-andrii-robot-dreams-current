using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LastLection"); 
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
        
        PlayerPrefs.DeleteAll();
    }

    public void OnStartGame()
    {
        
        ResetGameState();
        
        SceneManager.LoadScene("LastLection");
    }
}
