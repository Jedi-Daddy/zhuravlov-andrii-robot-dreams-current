using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damageAmount = 20;
    public int scorePerHit = 10; 

    private void Start() 
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Dragon dragon = other.GetComponent<Dragon>();

        if (dragon != null)
        {
            dragon.TakeDamage(damageAmount);

            // Add points for hitting
            ScoreManager.Instance.AddScore(scorePerHit);

            Debug.Log($"Hit! Added {scorePerHit} points. Current score: {ScoreManager.Instance.score}");
        }
        else
        {
            Debug.LogWarning("The bullet hit the object without Dragon: " + other.gameObject.name);
        }

        // Removing the bullet after impact
        Destroy(gameObject);
    }
}
