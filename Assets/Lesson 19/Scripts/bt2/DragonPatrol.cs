using UnityEngine;
using UnityEngine.AI;
using Unity.AI.BehaviorTree;

public class DragonPatrol : ActionNode
{
    private NavMeshAgent agent;
    private Transform[] patrolPoints;
    private int currentPoint = 0;

    protected override void OnStart()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        patrolPoints = blackboard.GetVariable<Transform[]>("PatrolPoints");
    }

    protected override NodeState OnUpdate()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return NodeState.Failure;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }

        return NodeState.Running;
    }
}
