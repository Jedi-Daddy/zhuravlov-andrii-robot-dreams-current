using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 30;
    public float speed = 10f;
    public float lifetime = 5f;
    public GameObject trailEffect; // ?? ���� ������� Particle System ������

    private GameObject spawnedEffect;

    private void Start()
    {
        Destroy(gameObject, lifetime);

        // ������� ������� � ���������� ��� � �� �� �������, ��� � �������
        if (trailEffect != null)
        {
            spawnedEffect = Instantiate(trailEffect, transform.position, transform.rotation);
            spawnedEffect.transform.SetParent(transform); // ����������� � ��������
        }
    }

    private void Update()
    {
        // �������� �����
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ������� ������� ������
        if (spawnedEffect != null)
        {
            spawnedEffect.transform.SetParent(null); // ����������� ����� ���������
            Destroy(spawnedEffect, 2f); // ��� 2 ������� �� "���������" �������
        }

        // ���� ������
        PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("������� ����� � ������ � ���� ����!");
        }

        // ������� �������
        Destroy(gameObject);
    }
}
