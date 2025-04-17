using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    [Header("Настройки взрыва")]
    public float explosionForce = 50f;
    public float explosionRadius = 5f;
    public float upwardModifier = 0.5f;
    public LayerMask explosionLayers;

    [Header("Визуальный эффект")]
    public GameObject explosionEffectPrefab;

    [Header("Камера игрока")]
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryExplodeFromCenter();
        }
    }

    private void TryExplodeFromCenter()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 explosionPoint = hit.point;

            // Добавление взрывной силы
            Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius, explosionLayers);
            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius, upwardModifier, ForceMode.Impulse);
                }
            }

            // ?? Создание эффекта взрыва
            if (explosionEffectPrefab != null)
            {
                GameObject effect = Instantiate(explosionEffectPrefab, explosionPoint, Quaternion.identity);
                Destroy(effect, 3f); // Удалить через 3 секунды
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
        }
    }
}
