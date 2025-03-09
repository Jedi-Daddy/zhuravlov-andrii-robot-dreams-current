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
        // ����� ���� ���������� ����������
        // ��������: Score = 0;
        // ���� ����������� �����-�� ������ � PlayerPrefs � ������ ��
        PlayerPrefs.DeleteAll();
    }
}
