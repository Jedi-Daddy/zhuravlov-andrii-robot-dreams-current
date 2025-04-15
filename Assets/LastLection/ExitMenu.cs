using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitPanel; // Панель подтверждения выхода

    private bool isPanelActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleExitPanel();
        }
    }

    public void ToggleExitPanel()
    {
        isPanelActive = !isPanelActive;
        exitPanel.SetActive(isPanelActive);

        if (isPanelActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ExitToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("LastMenu");
    }

    public void ClosePanel()
    {
        isPanelActive = false;
        exitPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
