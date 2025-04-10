using UnityEngine;
using UnityEngine.UI;

public class Trader : MonoBehaviour
{
    public GameObject traderPanel; // UI ������ �������
    public int medkitPrice = 5;    // ���� �������

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            traderPanel.SetActive(!traderPanel.activeSelf); // �������/������� ������
        }
    }

    public void BuyMedkit()
    {
        PlayerControllerIS player = FindObjectOfType<PlayerControllerIS>();

        if (player != null && player.coins >= medkitPrice)
        {
            player.coins -= medkitPrice;
            player.GetComponent<InventorySystem>().AddItem("Medkit");
            Debug.Log("������� �������!");
        }
        else
        {
            Debug.Log("�� ������� �����!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            traderPanel.SetActive(false); // ������� ��� ������
        }
    }

    public void TogglePanel()
    {
        traderPanel.SetActive(!traderPanel.activeSelf);
    }
}
