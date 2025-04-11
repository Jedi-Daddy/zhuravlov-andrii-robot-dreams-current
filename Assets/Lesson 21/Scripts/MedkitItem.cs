using UnityEngine;

public class MedkitItem : MonoBehaviour
{
    public void OnUseButtonClicked()
    {
        PlayerControllerIS player = FindObjectOfType<PlayerControllerIS>();

        if (player == null)
        {
            Debug.LogError("PlayerControllerIS not found on scene!");
            return;
        }

        if (player.curHp < player.maxHp)
        {
            player.curHp += 20;

            if (player.curHp > player.maxHp)
                player.curHp = player.maxHp;

            player.UpdateHPUI(); // ����� ����� ���������� UI ��
            Debug.Log("������� �� 20 HP. ������� HP: " + player.curHp);
        }
        else
        {
            Debug.Log("HP ��� ������!");
        }

        Destroy(gameObject); // ������� ������� ����� �������������
    }
}
