using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilAmount = 0.1f; // ���� ������
    public float recoilSpeed = 10f;   // �������� ��������������
    private Vector3 originalPosition; // �������� ������� ������

    private void Start()
    {
        originalPosition = transform.localPosition; // ���������� ��������� ������� ������
    }

    public void ApplyRecoil()
    {
        // ������� ������ ����� (������)
        transform.localPosition -= new Vector3(recoilAmount, 0, 0);
    }

    private void Update()
    {
        // ������� �������������� ������� ������
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, recoilSpeed * Time.deltaTime);
    }
}
