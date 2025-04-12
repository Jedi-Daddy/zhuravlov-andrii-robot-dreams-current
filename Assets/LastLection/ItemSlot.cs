using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image icon;          // ������ ��������
    [SerializeField] private Button useButton;    // ������ ������������
    [SerializeField] private Text itemNameText;   // ����� � ������ ��������

    public string ItemName { get; private set; }  // ��� �������� ������ �������
    private System.Action onUseCallback;          // ������� ��� ������������� ��������

    // ����� ��������� ���������� ��������
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

    // �������� ����
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
