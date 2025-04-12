using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private List<ItemSlot> itemSlots; // перетаскиваем в инспекторе!

    [Header("Icons")]
    [SerializeField] private Sprite medkitIcon;
    [SerializeField] private Sprite defaultIcon;

    private PlayerControllerIS player;
    private List<string> items = new List<string>();

    public GameObject inventoryPanel;

    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<PlayerControllerIS>();
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        RefreshUI();
    }

    public void RemoveItem(ItemSlot slot)
    {
        if (itemSlots.Contains(slot))
        {
            items.Remove(slot.ItemName);
            itemSlots.Remove(slot); // не забудь это тоже!
            Destroy(slot.gameObject);
        }
    }

    public void RefreshUI()
    {
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string item in items)
        {
            GameObject slotGO = Instantiate(itemSlotPrefab, slotsParent);
            ItemSlot itemSlot = slotGO.GetComponent<ItemSlot>();

            Sprite icon = GetItemIcon(item);

            if (item == "Medkit")
            {
                itemSlot.SetItem(item, icon, () =>
                {
                    player.Heal(20);
                    RemoveItem(itemSlot);
                });
            }
            else
            {
                itemSlot.SetItem(item, icon, () =>
                {
                    Debug.Log("Used: " + item);
                    RemoveItem(itemSlot);
                });
            }

            itemSlots.Add(itemSlot);
        }
    }

    private Sprite GetItemIcon(string item)
    {
        switch (item)
        {
            case "Medkit":
                return medkitIcon;
            default:
                return defaultIcon;
        }
    }
    public void AddMedkit()
    {
        GameObject slotObj = Instantiate(itemSlotPrefab, slotsParent);
        ItemSlot slot = slotObj.GetComponent<ItemSlot>();

        slot.SetItem("Medkit", medkitIcon, () => {
            PlayerControllerIS player = FindObjectOfType<PlayerControllerIS>();
            player.Heal(20); // Лечим на 20 хп
            RemoveItem(slot); // Удаляем аптечку после использования
        });
    }

    private bool isInventoryOpen = false;

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
