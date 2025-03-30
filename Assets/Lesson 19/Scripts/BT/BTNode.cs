using UnityEngine;
using XNode;

public abstract class BTNode : Node
{
    public enum NodeState { Running, Success, Failure }
    public abstract NodeState Evaluate();
}
