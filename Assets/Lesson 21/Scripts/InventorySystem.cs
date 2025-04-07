using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public GameObject inventoryPanel;      // Ссылка на панель UI инвентаря
    public Transform itemContainer;        // Контейнер для предметов (например, вертикальный список)
    public GameObject itemSlotPrefab;      // Префаб одного слота (с иконкой/названием)

    private List<string> inventoryItems = new List<string>();
    private bool isInventoryOpen = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
        Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isInventoryOpen;
    }

    public void AddItem(string itemName)
    {
        inventoryItems.Add(itemName);
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (string item in inventoryItems)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemContainer);
            slot.GetComponentInChildren<Text>().text = item;
        }
    }

    public bool HasItem(string itemName)
    {
        return inventoryItems.Contains(itemName);
    }

    public void RemoveItem(string itemName)
    {
        if (inventoryItems.Contains(itemName))
        {
            inventoryItems.Remove(itemName);
            RefreshUI();
        }
    }
}
