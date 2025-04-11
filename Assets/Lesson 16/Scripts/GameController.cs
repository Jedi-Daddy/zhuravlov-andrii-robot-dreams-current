using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private PlayerInput playerInput; 

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
    }

    void OnEnable()
    {
        
        playerInput.actions["Escape"].started += OnEscapePressed; 
    }

    void OnDisable()
    {
        
        playerInput.actions["Escape"].started -= OnEscapePressed;
    }

    private void ResetGameState()
    {
        
        PlayerPrefs.DeleteAll();
    }


    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.None;
        
        SceneManager.LoadScene("LastMenu");
        ResetGameState(); 
    }

}
