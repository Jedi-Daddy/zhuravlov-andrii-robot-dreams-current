using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Базовый класс узла
public abstract class BTNode
{
    public abstract bool Execute();
}

// Узел выбора (Selector)
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

// Последовательный узел (Sequence)
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

// Узлы действий
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

public class ShootNode : BTNode
{
    private Transform enemy, player;
    public ShootNode(Transform enemy, Transform player)
    {
        this.enemy = enemy;
        this.player = player;
    }
    public override bool Execute()
    {
        Debug.Log("Стреляю в игрока!");
        return true;
    }
}

public class MeleeAttackNode : BTNode
{
    private Transform enemy, player;
    public MeleeAttackNode(Transform enemy, Transform player)
    {
        this.enemy = enemy;
        this.player = player;
    }
    public override bool Execute()
    {
        Debug.Log("Бью игрока!");
        return true;
    }
}

// Узел проверки дистанции
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

// Основной AI-скрипт
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private BTNode root;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Узлы действий
        BTNode patrol = new PatrolNode(agent);
        BTNode shoot = new ShootNode(transform, player);
        BTNode melee = new MeleeAttackNode(transform, player);

        // Узлы условий
        BTNode isClose = new DistanceConditionNode(transform, player, 3f);
        BTNode isFar = new DistanceConditionNode(transform, player, 10f);

        // Дерево поведения
        root = new SelectorNode(new List<BTNode> {
            new SequenceNode(new List<BTNode> { isClose, melee }),
            new SequenceNode(new List<BTNode> { isFar, shoot }),
            patrol
        });
    }

    void Update()
    {
        root.Execute();
    }
}
