using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float speed = 20f; // �������� ����� ����
    public float damageAmount = 25f; // ����
    public Rigidbody rb; // ������ ����

    void Start()
    {
        // ���������, ���� �� Rigidbody (����� ���������)
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // ������� ���� �����
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(Mathf.RoundToInt(damageAmount));
        }

        // ���������� ���� ��� ���������
        Destroy(gameObject);
    }
}
