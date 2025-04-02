using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilAmount = 0.1f; // Сила отдачи
    public float recoilSpeed = 10f;   // Скорость восстановления
    private Vector3 originalPosition; // Исходная позиция оружия

    private void Start()
    {
        originalPosition = transform.localPosition; // Запоминаем начальную позицию оружия
    }

    public void ApplyRecoil()
    {
        // Смещаем оружие назад (отдача)
        transform.localPosition -= new Vector3(recoilAmount, 0, 0);
    }

    private void Update()
    {
        // Плавное восстановление позиции оружия
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, recoilSpeed * Time.deltaTime);
    }
}
