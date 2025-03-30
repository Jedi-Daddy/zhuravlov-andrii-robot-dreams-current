using UnityEngine;
using UnityEngine.AI;

[NodeTint("#1E90FF")] // Голубой цвет
public class BTPatrol : BTNode
{
    public Transform[] patrolPoints;
    private NavMeshAgent agent;
    private int currentPoint = 0;

    public override NodeState Evaluate()
    {
        if (agent == null) agent = GameObject.FindGameObjectWithTag("Dragon").GetComponent<NavMeshAgent>();

        if (patrolPoints.Length == 0) return NodeState.Failure;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
        return NodeState.Running;
    }
}
