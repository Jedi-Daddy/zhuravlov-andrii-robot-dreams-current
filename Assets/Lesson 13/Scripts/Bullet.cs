using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;  // ����� �����
    public float damage = 10f;   // ����

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("���� ������ �: " + collision.collider.name);
        Destroy(gameObject);
    }
}
