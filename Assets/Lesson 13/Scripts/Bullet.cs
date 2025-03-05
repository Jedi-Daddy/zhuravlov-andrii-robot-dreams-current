using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  
    public int damage = 10;   

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                int damage = 10;
                enemy.TakeDamage(damage);
            }
        }
        Debug.Log("Attack: " + collision.collider.name);
        Destroy(gameObject);
    }
}
