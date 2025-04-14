using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilAmount = 0.1f; // Сила отдачи
    public float recoilSpeed = 10f;   // Скорость восстановления
    private Vector3 originalPosition; // Исходная позиция оружия
    private Quaternion originalRotation; // Исходный поворот оружия

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void ApplyRecoil()
    {
        // Отдача назад по Z
        transform.localPosition -= new Vector3(0, 0, recoilAmount);
    }

    private void Update()
    {
        // Плавно возвращаем позицию и поворот оружия
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, recoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, recoilSpeed * Time.deltaTime);
    }
}
