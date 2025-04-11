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

            player.UpdateHPUI(); // чтобы сразу обновлялся UI хп
            Debug.Log("Лечение на 20 HP. Текущее HP: " + player.curHp);
        }
        else
        {
            Debug.Log("HP уже полное!");
        }

        Destroy(gameObject); // удаляем аптечку после использования
    }
}
