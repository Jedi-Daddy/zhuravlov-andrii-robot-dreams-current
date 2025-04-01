using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private int _damage = 15;

    private float speed = 30f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
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
        Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));

        Debug.Log($"���� ������ �: {other.gameObject.name}");

        // ����� EnemyAI �� ������� ������� ��� ��� ���������
        EnemyAI enemy = other.gameObject.GetComponentInParent<EnemyAI>();

        if (enemy != null)
        {
            Debug.Log($"��������� �� �����: {enemy.gameObject.name}, ������� ����: {_damage}");
            enemy.TakeDamage(_damage);

            // ��������� ���� �� ���������
            ScoreManager.Instance.AddScore(10); // ��������� 10 ����� �� ���������
        }
        else
        {
            Debug.Log("���� �� ������ �� ���� �������.");
        }

        Destroy(gameObject);
    }
}
