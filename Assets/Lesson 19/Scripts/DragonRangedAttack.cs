using UnityEngine;

public class DragonRangedAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float attackRange = 10f;
    public float attackCooldown = 3f;
    private float lastAttackTime;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            ShootFireball();
            lastAttackTime = Time.time;
        }
    }

    void ShootFireball()
    {
        Debug.Log("Дракон стреляет фаерболом!");
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }
}
