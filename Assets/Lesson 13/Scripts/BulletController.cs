using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;

    private float speed = 50f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);

        // —оздание следа от пули (децала)
        GameObject.Instantiate(bulletDecal, contact.point + contact.normal * .0001f, Quaternion.LookRotation(contact.normal));

        // ѕроверка на наличие компонента, который может получить урон
        if (other.gameObject.TryGetComponent(out PlayerController player)) // «амените на нужный класс
        {
            // Ќаносим урон игроку (например, 10 урона)
            player.TakeDamage(10); // ¬ам нужно будет передать параметр урона или добавить логику дл€ его вычислени€
        }

        // ”ничтожаем пулю после столкновени€
        Destroy(gameObject);
    }
}
