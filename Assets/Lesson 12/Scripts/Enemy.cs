using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100; // �������� �����

    // ����� ��������� �����
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("���� ������� ����: " + damage + ". ������� HP: " + health);

        // ���������, ���� �� ����
        if (health <= 0)
        {
            Die();
        }
    }

    // ����� ����������� �����
    void Die()
    {
        Debug.Log("���� ���������!");
        Destroy(gameObject);
    }
}
