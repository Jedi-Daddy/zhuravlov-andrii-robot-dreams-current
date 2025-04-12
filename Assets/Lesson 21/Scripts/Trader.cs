using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] private int medkitPrice = 5;   // Цена одной аптечки
    [SerializeField] private int maxMedkits = 5;    // Максимальное количество аптечек у игрока

    public void BuyMedkit()
    {
        PlayerControllerIS player = FindObjectOfType<PlayerControllerIS>();

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        // Проверяем, сколько у игрока уже есть аптечек
        int medkitCount = InventorySystem.Instance.GetItemCount("Medkit");

        if (medkitCount >= maxMedkits)
        {
            Debug.Log("У тебя уже максимум аптечек!");
            return;
        }

        if (player.Coins >= medkitPrice)
        {
            player.Coins -= medkitPrice;
            player.UpdateCoinsUI();

            InventorySystem.Instance.AddItem("Medkit");  // Добавляем медкит в инвентарь

            Debug.Log("Медкит куплен!");
        }
        else
        {
            Debug.Log("Недостаточно монет для покупки медкита!");
        }
    }
}
