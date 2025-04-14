using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilAmount = 0.1f; // ���� ������
    public float recoilSpeed = 10f;   // �������� ��������������
    private Vector3 originalPosition; // �������� ������� ������
    private Quaternion originalRotation; // �������� ������� ������

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void ApplyRecoil()
    {
        // ������ ����� �� Z
        transform.localPosition -= new Vector3(0, 0, recoilAmount);
    }

    private void Update()
    {
        // ������ ���������� ������� � ������� ������
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, recoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, recoilSpeed * Time.deltaTime);
    }
}
