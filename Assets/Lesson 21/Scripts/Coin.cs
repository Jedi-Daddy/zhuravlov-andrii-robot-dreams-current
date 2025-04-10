using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();
            if (player != null)
            {
                player.AddMoney(coinValue);
            }
            Destroy(gameObject);
        }
    }
}
