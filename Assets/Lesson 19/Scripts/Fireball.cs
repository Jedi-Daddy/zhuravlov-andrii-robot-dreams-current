using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 30;
    public float speed = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        // ���������� ������� ����� �������� �����, ���� �� �� ���������
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // �������� �����
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ������
        PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("������� ����� � ������ � ���� ����!");
        }

        // ����� �������� ������� ������������, ���� � �.�.

        Destroy(gameObject); // ���������� ������� � ����� ������ ����� ������������
    }
}

