using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float explosionRadius = 5f;    
    public float explosionForce = 700f;  
    public GameObject explosionEffect;  

    void Update()
    {
        
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            Vector3 explosionPoint = GetExplosionPoint();
            CreateExplosion(explosionPoint);
        }
    }

    Vector3 GetExplosionPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            return hit.point; 
        }
        return transform.position + transform.forward * 10f; 
    }

    void CreateExplosion(Vector3 position)
    {
        
        if (explosionEffect)
        {
            Instantiate(explosionEffect, position, Quaternion.identity);
        }

        
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float distance = Vector3.Distance(position, rb.position);
                float force = explosionForce * (1 - (distance / explosionRadius)); 
                rb.AddExplosionForce(force, position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }
    }
}