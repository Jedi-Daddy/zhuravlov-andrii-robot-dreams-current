using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }
}
