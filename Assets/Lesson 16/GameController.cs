using UnityEngine;
using UnityEngine.InputSystem; // ����� ���������� ��� ������ � ����� Input System
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private PlayerInput playerInput; // ��� ��������� ����� ����� InputSystem

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); // �������� ������ � ���������� PlayerInput
    }

    void OnEnable()
    {
        // ���������� ����� ��� ��������� ������� ������� Escape
        playerInput.actions["Escape"].started += OnEscapePressed; 
    }

    void OnDisable()
    {
        // ����������� ����������, ����� �������� ������ ������
        playerInput.actions["Escape"].started -= OnEscapePressed;
    }

    private void ResetGameState()
    {
        // ������ ������ ���� ���������� ����������
        // ��������: Score = 0;
        // ���� ����������� �����-�� ������ � PlayerPrefs � ������ ��
        PlayerPrefs.DeleteAll();
    }


    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        // ����� ������ ������� Escape, ��������� ������� �����
        SceneManager.LoadScene("MenuLection16");
        ResetGameState(); // ���������� ��������� ����
    }

}
