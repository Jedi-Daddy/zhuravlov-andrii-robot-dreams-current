using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarFill; // ������ �� �������� ����� ��������

    public void UpdateHealth(float currentHP, float maxHP)
    {
        HealthBarFill.fillAmount = Mathf.Clamp01(currentHP / maxHP);
    }
}