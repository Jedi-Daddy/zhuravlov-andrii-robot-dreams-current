using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100; // Здоровье врага

    // Метод получения урона
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Враг получил урон: " + damage + ". Текущее HP: " + health);

        // Проверяем, умер ли враг
        if (health <= 0)
        {
            Die();
        }
    }

    // Метод уничтожения врага
    void Die()
    {
        Debug.Log("Враг уничтожен!");
        Destroy(gameObject);
    }
}
