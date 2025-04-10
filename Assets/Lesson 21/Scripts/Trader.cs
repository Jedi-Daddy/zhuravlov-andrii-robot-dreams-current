using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private int medkitPrice = 5;
    [SerializeField] private InventorySystem inventorySystem;

    public void BuyMedkit()
    {
        PlayerControllerIS player = FindObjectOfType<PlayerControllerIS>();

        if (player == null)
        {
            Debug.LogError("Trader: �� ������ PlayerControllerIS �� �����!");
            return;
        }

        if (player.coins >= medkitPrice)
        {
            player.coins -= medkitPrice;
            player.SendMessage("UpdateCoinsUI");

            inventorySystem.AddItem("Medkit");

            Debug.Log("������� ������� �������!");
        }
        else
        {
            Debug.Log("������������ ����� ��� ������� �������!");
        }
    }
}
