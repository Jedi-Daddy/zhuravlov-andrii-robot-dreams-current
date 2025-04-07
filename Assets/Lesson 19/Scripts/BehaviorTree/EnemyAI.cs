using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// ������� ����� ����
public abstract class BTNode
{
    public abstract bool Execute();
}

// ���� ������ (Selector)
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

// ���������������� ���� (Sequence)
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

// ���� ��������������
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

// ���� �������� (Fireball)
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
        // ��������: ���������� ������ �����������, ������� Y ����������
        Vector3 direction = (player.position - enemy.position).normalized;

        // ��������� ������� ����� ��� �������� � 3D ������������
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * 5f);

        // ����� ��������� ������� ����� ��������, ����� ��� ���� ���������� ����� �� ������
        firePoint.rotation = Quaternion.LookRotation(direction);

        if (canShoot)
        {
            Debug.Log("����� Fireball � ������!");

            GameObject fireball = GameObject.Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = fireball.GetComponent<Rigidbody>();

            // ���������� ����������� �� ����� �������� � ������ ��� ����������� �������� �������
            rb.velocity = direction * fireballSpeed;

            canShoot = false;
            enemy.GetComponent<EnemyAI>().StartCoroutine(ResetShoot());
        }
        return true;
    }

    private System.Collections.IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}

// ���� ������� �����
public class MeleeAttackNode : BTNode
{
    private Transform enemy, player;
    private Transform attackHand; // "����" - ���
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
        this.attackPosition = originalPosition + new Vector3(0, -0.5f, 0); // ������� ����
    }

    public override bool Execute()
    {
        // ������������ ����� � ������
        Vector3 direction = (player.position - enemy.position).normalized;
        direction.y = 0; // ��������� �������� �� ��� Y
        enemy.rotation = Quaternion.LookRotation(direction);
        if (canAttack)
        {
            Debug.Log("��� ������ �����!");
            enemy.GetComponent<EnemyAI>().StartCoroutine(Attack());
        }
        return true;
    }

    private System.Collections.IEnumerator Attack()
    {
        canAttack = false;

        // ������� "����" ����
        float elapsedTime = 0;
        while (elapsedTime < 0.2f)
        {
            attackHand.localPosition = Vector3.Lerp(originalPosition, attackPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime * attackSpeed;
            yield return null;
        }
        attackHand.localPosition = attackPosition;

        // ���������, ����� �� ���� � ������
        if (Vector3.Distance(enemy.position, player.position) <= 3f)
        {
            PlayerControllerIS playerHealth = player.GetComponent<PlayerControllerIS>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // ������� "����" ������� �����
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

// ���� �������� ���������
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

// �������� AI-������
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public Transform firePoint;
    public GameObject fireballPrefab;
    private BTNode root;

    public int maxHP = 100; // ������������ ��������
    private int currentHP;   // ������� ��������

    public Image hpBarFill;  // ����������� ����� ��� HP Bar (������ �� Image)

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP; // ������������� �������� �� ��������

        if (hpBarFill != null)
        {
            // ������������� ������������ ������ ������
            hpBarFill.fillAmount = 1f;  // ������� HP ������
        }

        if (player == null)
        {
            Debug.LogError("����� �� �������� � EnemyAI! ������� ��� � ����������.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("firePoint (����� ��������) �� ���������!");
            return;
        }

        if (fireballPrefab == null)
        {
            Debug.LogError("fireballPrefab (�������� ���) �� ��������!");
            return;
        }

        Transform attackHand = transform.Find("AttackHand");
        if (attackHand == null)
        {
            Debug.LogError("AttackHand (����) �� �������! �������, ��� � ����� ���� ������ � ����� ������.");
            return;
        }

        BTNode patrol = new PatrolNode(agent);
        BTNode shoot = new ShootNode(transform, player, firePoint, fireballPrefab);
        BTNode melee = new MeleeAttackNode(transform, player, attackHand);

        BTNode isClose = new DistanceConditionNode(transform, player, 3f);
        BTNode isFar = new DistanceConditionNode(transform, player, 10f);

        root = new SelectorNode(new List<BTNode> {
        new SequenceNode(new List<BTNode> { isClose, melee }),
        new SequenceNode(new List<BTNode> { isFar, shoot }),
        patrol
    });

        Debug.Log("������ ��������� ������� �������!");
    }

    void Update()
    {
        if (currentHP > 0)
        {
            root.Execute();
        }
    }
    // ����� ��������� �����
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        if (hpBarFill != null)
        {
            // �������� ���������� ������ � ����������� �� �������� HP
            hpBarFill.fillAmount = (float)currentHP / (float)maxHP;
        }

        Debug.Log($"���� ������� {damage} �����! HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // ����� ������
    private void Die()
    {
        Debug.Log("���� �����!");
        Destroy(gameObject); // ������� ����� �� �����
    }
}