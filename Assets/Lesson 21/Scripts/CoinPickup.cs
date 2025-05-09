using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // ������� ����� ��� ��� �������

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();

            if (player != null)
            {
                player.AddCoins(coinValue); // ��������� ������� ����� ����� �����
            }

            Destroy(gameObject); // ������� ������ ����� �������
        }
    }
}
