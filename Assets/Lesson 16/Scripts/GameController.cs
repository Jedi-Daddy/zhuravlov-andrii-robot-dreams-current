using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private GameObject exitPanel; // Панель подтверждения выхода
    private bool isPanelActive = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        playerInput.actions["Escape"].performed += OnEscapePressed;
    }

    void OnDisable()
    {
        playerInput.actions["Escape"].performed += OnEscapePressed;
    }

    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        isPanelActive = !isPanelActive;
        exitPanel.SetActive(isPanelActive);

        Cursor.lockState = isPanelActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ExitToMenu()  
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene("LastMenu");
    }

    public void CloseExitPanel() 
    {
        isPanelActive = false;
        exitPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
