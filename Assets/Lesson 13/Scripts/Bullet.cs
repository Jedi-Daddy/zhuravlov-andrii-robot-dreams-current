using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damageAmount = 20;

    private void Star()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.GetComponent<Rigidbody>());
        if (other.tag == "Dragon")
        {
            transform.parent = other.transform;
            other.GetComponent<Dragon>().TakeDamage(damageAmount);
        }
    }
}
