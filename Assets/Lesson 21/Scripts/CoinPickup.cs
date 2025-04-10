using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // сколько валюты даёт монетка

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();
            if (player != null)
            {
                player.AddMoney(coinValue); // вызываем метод добавления валюты
            }

            Destroy(gameObject); // удаляем монету после подбора
        }
    }
}
