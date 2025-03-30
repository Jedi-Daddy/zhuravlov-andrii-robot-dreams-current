using System.Collections.Generic;
using UnityEngine;

[NodeTint("#228B22")] // Зеленый цвет в редакторе
public class BTSequence : BTNode
{
    [Input] public List<BTNode> children;

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            if (child.Evaluate() == NodeState.Failure)
                return NodeState.Failure;
        }
        return NodeState.Success;
    }
}
