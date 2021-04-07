using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_Sequence : B05_Node
{
    protected List<B05_Node> nodes = new List<B05_Node>();
    protected int cur;

    public B05_Sequence(List<B05_Node> n)
    {
        nodes = n;
        cur = 0;
    }

    public override NodeState Evaluate()
    {
        if (cur == nodes.Count)
            cur = 0;

        switch(nodes[cur].Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                return _nodeState;
            case NodeState.SUCCESS:
                ++cur;
                break;
            case NodeState.FAILURE:
                _nodeState = NodeState.FAILURE;
                return _nodeState;
        }

        _nodeState = (cur == nodes.Count) ? NodeState.SUCCESS : NodeState.RUNNING;
        return _nodeState;
    }
}
