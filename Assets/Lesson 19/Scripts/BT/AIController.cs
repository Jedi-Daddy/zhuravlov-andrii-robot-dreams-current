using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController instance;

    [Header("AI Settings")]
    public float meleeRange = 2f;      // Дистанция для ближней атаки
    public float rangedRange = 10f;    // Дистанция для файербола
    public float patrolSpeed = 3f;     // Скорость патрулирования
    public GameObject fireballPrefab;  // Префаб файербола
    public Transform fireballSpawn;    // Точка спавна файербола
    public Transform[] patrolPoints;   // Точки патрулирования

    private int currentPatrolIndex = 0;
    private Transform player;
    private UnityEngine.AI.NavMeshAgent agent;
    public DragonBT behaviourTree;

    void Awake()
    {
        instance = this;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (behaviourTree != null && behaviourTree.nodes.Count > 0)
        {
            (behaviourTree.nodes[0] as BTNode)?.Execute();
        }
    }


    // === Патрулирование ===
    public void MoveTo(Vector3 target)
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(target);
    }

    public bool ReachedPatrolPoint()
    {
        return !agent.pathPending && agent.remainingDistance < 1f;
    }

    public void NextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    public Vector3 GetCurrentPatrolPoint()
    {
        return patrolPoints[currentPatrolIndex].position;
    }

    // === Ближняя атака ===
    public bool CanMeleeAttack()
    {
        return Vector3.Distance(transform.position, player.position) <= meleeRange;
    }

    public void PerformMeleeAttack()
    {
        Debug.Log("Дракон наносит удар когтями!");
        player.GetComponent<PlayerControllerIS>().TakeDamage(20);
    }

    // === Дальняя атака ===
    public bool CanShootFireball()
    {
        return Vector3.Distance(transform.position, player.position) <= rangedRange;
    }

    public void ShootFireball()
    {
        Debug.Log("Дракон выпускает файербол!");
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawn.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody>().velocity = (player.position - fireballSpawn.position).normalized * 10f;
    }
}
