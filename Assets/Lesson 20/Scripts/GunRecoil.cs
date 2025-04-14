using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] private Transform recoilPoint; // ���� ������ BlasterHolder
    [SerializeField] private float recoilAmount = 0.1f;
    [SerializeField] private float recoilRecoverySpeed = 10f;

    private Vector3 originalPosition;

    private void Start()
    {
        if (recoilPoint == null)
        {
            Debug.LogError("RecoilPoint �� �������� � ����������!");
            return;
        }

        originalPosition = recoilPoint.localPosition;
    }

    public void ApplyRecoil()
    {
        // ������ ������ �� ��������� ��� �����
        recoilPoint.localPosition -= new Vector3(0, 0, recoilAmount);
    }

    private void Update()
    {
        // ������� ����������� � �������� �������
        recoilPoint.localPosition = Vector3.Lerp(recoilPoint.localPosition, originalPosition, recoilRecoverySpeed * Time.deltaTime);
    }
}
