using UnityEngine;

[NodeTint("#8B0000")] // Темно-красный
public class BTRangedAttack : BTNode
{
    public GameObject fireballPrefab;
    public float fireballRange = 10f;
    private Transform player;
    [SerializeField] private float attackRange = 20f; // Дальность атаки

    public override NodeState Evaluate()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = Vector3.Distance(player.position, GameObject.FindGameObjectWithTag("Dragon").transform.position);
        if (distance > attackRange && distance <= fireballRange)
        {
            GameObject fireball = GameObject.Instantiate(fireballPrefab, GameObject.FindGameObjectWithTag("Dragon").transform.position, Quaternion.identity);
            fireball.GetComponent<Rigidbody>().velocity = (player.position - fireball.transform.position).normalized * 10f;

            Debug.Log("Дракон запускает фаербол!");
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
