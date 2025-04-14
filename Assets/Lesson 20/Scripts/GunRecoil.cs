using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] private Transform recoilPoint; // —юда ставим BlasterHolder
    [SerializeField] private float recoilAmount = 0.1f;
    [SerializeField] private float recoilRecoverySpeed = 10f;

    private Vector3 originalPosition;

    private void Start()
    {
        if (recoilPoint == null)
        {
            Debug.LogError("RecoilPoint не назначен в инспекторе!");
            return;
        }

        originalPosition = recoilPoint.localPosition;
    }

    public void ApplyRecoil()
    {
        // ќтдача строго по локальной оси назад
        recoilPoint.localPosition -= new Vector3(0, 0, recoilAmount);
    }

    private void Update()
    {
        // ѕлавное возвращение в исходную позицию
        recoilPoint.localPosition = Vector3.Lerp(recoilPoint.localPosition, originalPosition, recoilRecoverySpeed * Time.deltaTime);
    }
}
