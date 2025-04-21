using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class EnemyAnimatorFSM : MonoBehaviour
{
    private enum EnemyAnimState
    {
        Idle,
        Run
    }

    private EnemyAnimState currentState = EnemyAnimState.Idle;
    private Animator animator;
    private NavMeshAgent agent;
    private bool hasTouchedGround = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!IsGrounded())
        {
            ChangeState(EnemyAnimState.Idle); // или Fall, если есть
            return;
        }

        if (!agent.enabled || !agent.isOnNavMesh)
        {
            ChangeState(EnemyAnimState.Idle);
            return;
        }

        float speed = agent.velocity.magnitude;

        if (speed > 0.1f)
        {
            ChangeState(EnemyAnimState.Run);
        }
        else
        {
            ChangeState(EnemyAnimState.Idle);
        }
    }

    private void ChangeState(EnemyAnimState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (newState)
        {
            case EnemyAnimState.Idle:
                animator.CrossFade("IdlePistol", 0.1f);
                break;
            case EnemyAnimState.Run:
                animator.CrossFade("Run", 0.1f);
                break;
        }
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float rayLength = 1.2f;
        Debug.DrawRay(origin, Vector3.down * rayLength, Color.red);
        return Physics.Raycast(origin, Vector3.down, rayLength);
    }
}
