using UnityEngine;

public class MedkitPickup : MonoBehaviour
{
    [SerializeField] private string itemName = "Medkit";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventorySystem.Instance.AddItem(itemName);
            Debug.Log("Picked up medkit!");
            Destroy(gameObject);
        }
    }
}
