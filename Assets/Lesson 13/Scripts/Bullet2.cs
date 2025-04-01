using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damageAmount = 15;
    public int scorePerHit = 10; 

    private void Start() 
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>()?.TakeDamage(15);
            Destroy(gameObject); // Удаляем пулю
        }
    }
}
