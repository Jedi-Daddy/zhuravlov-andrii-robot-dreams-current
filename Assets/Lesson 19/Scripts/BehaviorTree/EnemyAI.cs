using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public abstract class BTNode
{
    public abstract bool Execute();
}

public class SelectorNode : BTNode
{
    private List<BTNode> children;
    public SelectorNode(List<BTNode> children) { this.children = children; }
    public override bool Execute()
    {
        foreach (var child in children)
        {
            if (child.Execute()) return true;
        }
        return false;
    }
}

public class SequenceNode : BTNode
{
    private List<BTNode> children;
    public SequenceNode(List<BTNode> children) { this.children = children; }
    public override bool Execute()
    {
        foreach (var child in children)
        {
            if (!child.Execute()) return false;
        }
        return true;
    }
}

public class PatrolNode : BTNode
{
    private NavMeshAgent agent;
    public PatrolNode(NavMeshAgent agent) { this.agent = agent; }
    public override bool Execute()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 randomPoint = agent.transform.position + Random.insideUnitSphere * 5f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        return true;
    }
}

public class ChaseNode : BTNode
{
    private NavMeshAgent agent;
    private Transform player;
    public ChaseNode(NavMeshAgent agent, Transform player)
    {
        this.agent = agent;
        this.player = player;
    }

    public override bool Execute()
    {
        agent.SetDestination(player.position);
        return true;
    }
}

public class ShootNode : BTNode
{
    private Transform enemy, player, firePoint;
    private GameObject fireballPrefab;
    private float fireballSpeed = 10f;
    private float fireRate = 1.5f;
    private bool canShoot = true;

    public ShootNode(Transform enemy, Transform player, Transform firePoint, GameObject fireballPrefab)
    {
        this.enemy = enemy;
        this.player = player;
        this.firePoint = firePoint;
        this.fireballPrefab = fireballPrefab;
    }

    public override bool Execute()
    {
        // ����� ���� ������ � ����
        Vector3 targetPoint = player.position + Vector3.up * (-0.5f);
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        Debug.DrawRay(firePoint.position, direction * 5f, Color.red, 1f);

        // ��������� enemy � firePoint � ������� ����
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * 5f);
        firePoint.rotation = lookRotation;

        if (canShoot)
        {
            GameObject fireball = GameObject.Instantiate(fireballPrefab, firePoint.position, Quaternion.LookRotation(direction));
            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            rb.velocity = direction * fireballSpeed;

            canShoot = false;
            enemy.GetComponent<EnemyAI>().StartCoroutine(ResetShoot());
        }

        return true;
    }

    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}

public class MeleeAttackNode : BTNode
{
    private Transform enemy, player;
    private Transform attackHand;
    private Vector3 originalPosition;
    private Vector3 attackPosition;
    private float attackSpeed = 5f;
    private float attackRate = 1.5f;
    private bool canAttack = true;
    private int damage = 10;

    public MeleeAttackNode(Transform enemy, Transform player, Transform attackHand)
    {
        this.enemy = enemy;
        this.player = player;
        this.attackHand = attackHand;
        this.originalPosition = attackHand.localPosition;
        this.attackPosition = originalPosition + new Vector3(0, -0.5f, 0);
    }

    public override bool Execute()
    {
        Vector3 direction = (player.position - enemy.position).normalized;
        direction.y = 0;
        enemy.rotation = Quaternion.LookRotation(direction);
        if (canAttack)
        {
            enemy.GetComponent<EnemyAI>().StartCoroutine(Attack());
        }
        return true;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        float elapsedTime = 0;
        while (elapsedTime < 0.2f)
        {
            attackHand.localPosition = Vector3.Lerp(originalPosition, attackPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime * attackSpeed;
            yield return null;
        }
        attackHand.localPosition = attackPosition;

        if (Vector3.Distance(enemy.position, player.position) <= 3f)
        {
            PlayerControllerIS playerHealth = player.GetComponent<PlayerControllerIS>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        elapsedTime = 0;
        while (elapsedTime < 0.2f)
        {
            attackHand.localPosition = Vector3.Lerp(attackPosition, originalPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime * attackSpeed;
            yield return null;
        }
        attackHand.localPosition = originalPosition;

        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }
}

public class DistanceConditionNode : BTNode
{
    private Transform enemy, player;
    private float range;
    public DistanceConditionNode(Transform enemy, Transform player, float range)
    {
        this.enemy = enemy;
        this.player = player;
        this.range = range;
    }
    public override bool Execute()
    {
        return Vector3.Distance(enemy.position, player.position) <= range;
    }
}

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform firePoint;
    public GameObject fireballPrefab;
    public GameObject coinPrefab;
    public Image hpBarFill;

    [Header("Enemy Stats")]
    public int maxHP = 100;
    private int currentHP;

    [Header("AI Ranges")]
    public float meleeRange = 3f;
    public float detectionRange = 10f;
    public float chaseExitDistance = 20f;

    [Header("Events")]
    public UnityEvent onDeath;

    [Header("��������")]
    private Animator animator;

    private NavMeshAgent agent;
    private BTNode root;

    private Renderer enemyRenderer;
    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;

        if (hpBarFill != null)
            hpBarFill.fillAmount = 1f;

        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;

        Transform attackHand = transform.Find("AttackHand");
        if (player == null || firePoint == null || fireballPrefab == null || attackHand == null)
        {
            Debug.LogError("EnemyAI: ������� ��� ������ (player, firePoint, prefab, AttackHand)");
            return;
        }

        BTNode patrol = new PatrolNode(agent);
        BTNode chase = new ChaseNode(agent, player);
        BTNode shoot = new ShootNode(transform, player, firePoint, fireballPrefab);
        BTNode melee = new MeleeAttackNode(transform, player, attackHand);

        BTNode isClose = new DistanceConditionNode(transform, player, meleeRange);
        BTNode isDetected = new DistanceConditionNode(transform, player, detectionRange);
        BTNode isWithinChase = new DistanceConditionNode(transform, player, chaseExitDistance);

        root = new SelectorNode(new List<BTNode> {
            new SequenceNode(new List<BTNode> { isClose, melee }),
            new SequenceNode(new List<BTNode> { isDetected, chase, shoot }),
            patrol
        });
    }

    void Update()
    {
        if (currentHP > 0)
        {
            root.Execute();

            // ��������� �������� �����
            if (animator != null)
            {
                animator.CrossFade("Run", 0.1f);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        if (hpBarFill != null)
            hpBarFill.fillAmount = (float)currentHP / maxHP;

        HitFlash();

        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    public void SpawnCoin()
    {
        if (coinPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 0.2f;
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void HitFlash()
    {
        if (!isFlashing && enemyRenderer != null)
            StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        isFlashing = true;
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material.color = originalColor;
        isFlashing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseExitDistance);
    }
}