using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private int _damage = 15;
    [SerializeField] private GameObject trailEffect;

    private float speed = 30f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);

        // Спавним трейл, если есть, и направляем его по вектору движения
        if (trailEffect != null)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            GameObject effect = Instantiate(trailEffect, transform.position, lookRotation);
            effect.transform.SetParent(transform);
        }
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);

        EnemyAI enemy = other.gameObject.GetComponentInParent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
            ScoreManager.Instance.AddScore(10);
            enemy.StartCoroutine(HitEffect(enemy));
        }
        else
        {
            GameObject decal = Instantiate(bulletDecal, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
            Destroy(decal, 5f);
        }

        Destroy(gameObject);
    }

    private IEnumerator HitEffect(EnemyAI enemy)
    {
        Renderer rend = enemy.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Color originalColor = rend.material.color;
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = originalColor;
        }
    }
}
