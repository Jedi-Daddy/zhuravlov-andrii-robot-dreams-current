using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;          // Иконка предмета
    [SerializeField] private Button useButton;    // Кнопка Использовать
    [SerializeField] private Text itemNameText;   // Текст с именем предмета

    public string ItemName { get; private set; }  // Имя предмета внутри скрипта
    private System.Action onUseCallback;          // Коллбек при использовании предмета

    // Метод установки параметров предмета
    public void SetItem(string name, Sprite newIcon, System.Action onUse)
    {
        ItemName = name;
        icon.sprite = newIcon;
        onUseCallback = onUse;

        if (itemNameText != null)
            itemNameText.text = name;

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(() => onUseCallback?.Invoke());
    }

    // Очистить слот
    public void ClearSlot()
    {
        ItemName = null;

        if (icon != null)
            icon.sprite = null;

        if (itemNameText != null)
            itemNameText.text = "";

        useButton.onClick.RemoveAllListeners();
    }
}
