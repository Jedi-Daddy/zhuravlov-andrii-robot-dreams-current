using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damageAmount = 20;
    public int scorePerHit = 10; 

    private void Start() 
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.GetComponent<Rigidbody>());

        if (other.CompareTag("Dragon")) 
        {
            transform.parent = other.transform;
            other.GetComponent<Dragon>().TakeDamage(damageAmount);

            
            ScoreManager.instance.AddScore(scorePerHit);
        }
    }
}
