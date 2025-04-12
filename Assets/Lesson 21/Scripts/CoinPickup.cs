using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // Сколько монет даёт эта монетка

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();

            if (player != null)
            {
                player.AddCoins(coinValue); // Добавляем монетки через новый метод
            }

            Destroy(gameObject); // Удаляем монету после подбора
        }
    }
}
