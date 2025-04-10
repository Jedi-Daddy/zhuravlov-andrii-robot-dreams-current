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
            Debug.LogError("Trader: Не найден PlayerControllerIS на сцене!");
            return;
        }

        if (player.coins >= medkitPrice)
        {
            player.coins -= medkitPrice;
            player.SendMessage("UpdateCoinsUI");

            inventorySystem.AddItem("Medkit");

            Debug.Log("Аптечка успешно куплена!");
        }
        else
        {
            Debug.Log("Недостаточно монет для покупки аптечки!");
        }
    }
}
