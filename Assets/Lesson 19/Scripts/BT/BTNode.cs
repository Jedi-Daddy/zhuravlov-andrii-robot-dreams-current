using UnityEngine;
using XNode;

public abstract class BTNode : Node
{
    [Output] public BTNode next;
    public abstract bool Execute();
}
