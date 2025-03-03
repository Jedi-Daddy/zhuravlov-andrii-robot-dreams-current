using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // Время жизни
    public float damage = 10f;   // Урон

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Пуля попала в: " + collision.collider.name);
        Destroy(gameObject);
    }
}
