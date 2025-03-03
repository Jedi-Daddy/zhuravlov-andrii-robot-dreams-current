using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float speed = 20f; // Скорость полёта пули
    public float damageAmount = 25f; // Урон
    public Rigidbody rb; // Физика пули

    void Start()
    {
        // Проверяем, есть ли Rigidbody (иначе добавляем)
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Двигаем пулю вперёд
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(Mathf.RoundToInt(damageAmount));
        }

        // Уничтожаем пулю при попадании
        Destroy(gameObject);
    }
}
