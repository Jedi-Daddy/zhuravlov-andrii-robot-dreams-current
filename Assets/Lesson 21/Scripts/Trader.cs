using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private int medkitPrice = 5; // ���� ����� �������

    public void BuyMedkit()
    {
        PlayerControllerIS player = FindObjectOfType<PlayerControllerIS>();

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        if (player.Coins >= medkitPrice)
        {
            player.Coins -= medkitPrice;
            player.UpdateCoinsUI();

            InventorySystem.Instance.AddItem("Medkit");  // ��������� ������ � ���������

            Debug.Log("������ ������!");
        }
        else
        {
            Debug.Log("������������ ����� ��� ������� �������!");
        }
    }
}
