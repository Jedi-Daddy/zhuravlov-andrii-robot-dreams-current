using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosionController : MonoBehaviour
{
    public Camera playerCamera;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CreateExplosion();
        }
    }

    void CreateExplosion()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 explosionPoint = hit.point;
            Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);

            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.attachedRigidbody;
                if (rb != null)
                {
                    float distance = Vector3.Distance(explosionPoint, rb.position);
                    float forceMultiplier = 1 - (distance / explosionRadius);
                    rb.AddExplosionForce(explosionForce * forceMultiplier, explosionPoint, explosionRadius);
                }
            }
        }
    }
}
