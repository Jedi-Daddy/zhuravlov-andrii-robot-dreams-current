using UnityEngine;

[NodeTint("#FF4500")] // ��������� ����
public class BTMeleeAttack : BTNode
{
    public int attackDamage = 20;
    public float attackRange = 2f;
    private Transform player;

    public override NodeState Evaluate()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = Vector3.Distance(player.position, GameObject.FindGameObjectWithTag("Dragon").transform.position);
        if (distance <= attackRange)
        {
            player.GetComponent<PlayerControllerIS>().TakeDamage(attackDamage);
            Debug.Log("������ ������� � ������� ���!");
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
