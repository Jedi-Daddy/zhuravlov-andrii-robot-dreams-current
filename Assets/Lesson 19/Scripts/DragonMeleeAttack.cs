using UnityEngine;

public class DragonMeleeAttack : MonoBehaviour
{
    public int attackDamage = 20;
    public float attackRange = 2f;
    public float attackCooldown = 3f;
    private float lastAttackTime;

    private Transform player;
    private PlayerControllerIS playerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerControllerIS>();
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"Дракон атакует! Игрок получил {attackDamage} урона. Текущее ХП: {playerHealth.curHp}");
        }
    }
}
