using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 30;
    public float speed = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        // Уничтожить фаербол через заданное время, если он не столкнётся
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Движение вперёд
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Урон игроку
        PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("Фаербол попал в игрока и нанёс урон!");
        }

        // Можно добавить эффекты столкновения, звук и т.д.

        Destroy(gameObject); // Уничтожаем фаербол в любом случае после столкновения
    }
}

