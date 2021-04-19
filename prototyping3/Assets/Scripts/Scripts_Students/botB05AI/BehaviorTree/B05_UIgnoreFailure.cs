using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_UIgnoreFailure : B05_UNode
{
    private B05_UNode node;

    private float timer;
    private const float MAX_TIME = 2.0f;

    public B05_UIgnoreFailure(B05_UNode node)
    {
        this.node = node;
        timer = Time.fixedTime;
    }

    public override NodeState Evaluate()
    {
        if (Time.fixedTime - timer > MAX_TIME)
            _nodeState = node.Evaluate();

        if (_nodeState == NodeState.FAILURE)
        {
            _nodeState = NodeState.SUCCESS;
            timer = Time.fixedTime;
        }

        return _nodeState;
    }

    public override float CalcScore()
    {
        return node.CalcScore();
    }
}
