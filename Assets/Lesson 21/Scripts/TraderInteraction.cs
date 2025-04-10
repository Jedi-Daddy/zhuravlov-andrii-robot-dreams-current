using UnityEngine;

public class TraderInteraction : MonoBehaviour
{
    [SerializeField] private GameObject pressEText;    // UI надпись "Нажми E"
    [SerializeField] private GameObject traderPanel;   // Панель покупки
    [SerializeField] private float interactDistance = 3f; // Радиус активации
    [SerializeField] private Transform player;         // Игрок

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
        traderPanel.SetActive(!traderPanel.activeSelf);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}
