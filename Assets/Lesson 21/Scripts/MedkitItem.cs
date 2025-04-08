using UnityEngine;

public class MedkitItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 30;

    public void Use()
    {
        if (PlayerControllerIS.Instance != null)
        {
            PlayerControllerIS.Instance.Heal(healAmount);
            InventorySystem.Instance.RemoveItem("Medkit");
            Debug.Log("Used Medkit and healed player for " + healAmount);
        }
        else
        {
            Debug.LogWarning("Player instance not found for healing");
        }
    }
}
