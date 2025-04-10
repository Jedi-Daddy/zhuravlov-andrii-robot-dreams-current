using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int coins = 0;
    public TextMeshProUGUI coinsText;  // UI Text для монет

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + coins;
    }
}
