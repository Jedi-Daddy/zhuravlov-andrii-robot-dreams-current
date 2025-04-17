using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage = 30;
    public float speed = 10f;
    public float lifetime = 5f;
    public GameObject trailEffect; // ?? Сюда назначь Particle System префаб

    private GameObject spawnedEffect;

    private void Start()
    {
        Destroy(gameObject, lifetime);

        // Спавним партикл и направляем его в ту же сторону, что и фаербол
        if (trailEffect != null)
        {
            spawnedEffect = Instantiate(trailEffect, transform.position, transform.rotation);
            spawnedEffect.transform.SetParent(transform); // Привязываем к фаерболу
        }
    }

    private void Update()
    {
        // Движение вперёд
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Удаляем партикл эффект
        if (spawnedEffect != null)
        {
            spawnedEffect.transform.SetParent(null); // Отсоединяем перед удалением
            Destroy(spawnedEffect, 2f); // Даём 2 секунды на "затухание" эффекта
        }

        // Урон игроку
        PlayerControllerIS player = other.GetComponent<PlayerControllerIS>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("Фаербол попал в игрока и нанёс урон!");
        }

        // Удаляем фаербол
        Destroy(gameObject);
    }
}
