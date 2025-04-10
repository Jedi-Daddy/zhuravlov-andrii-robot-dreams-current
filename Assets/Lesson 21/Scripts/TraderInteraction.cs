using UnityEngine;

public class TraderInteraction : MonoBehaviour
{
    [SerializeField] private GameObject pressEText;    // UI ������� "����� E"
    [SerializeField] private GameObject traderPanel;   // ������ �������
    [SerializeField] private float interactDistance = 3f; // ������ ���������
    [SerializeField] private Transform player;         // �����

    private bool isPlayerNear = false;

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactDistance)
        {
            pressEText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleTraderPanel();
            }
        }
        else
        {
            pressEText.SetActive(false);
            if (traderPanel.activeSelf)
                traderPanel.SetActive(false);
        }
    }

    private void ToggleTraderPanel()
    {
        bool isActive = !traderPanel.activeSelf;
        traderPanel.SetActive(isActive);

        if (isActive)
        {
            // ��������� ������ � ������� ��� �������
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // ������������� ������ ������� � ������ ���
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
