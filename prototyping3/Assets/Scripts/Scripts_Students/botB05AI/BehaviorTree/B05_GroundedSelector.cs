using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_GroundedSelector : B05_Node
{
    protected B05_Node groundedNode;
    protected B05_Node ungroundedNode;
    protected Bot05_Move bot;

    private bool b_isRunning;
    private bool b_runningNormal;

    public B05_GroundedSelector(B05_Node groundNode, B05_Node airNode, Bot05_Move bot)
    {
        groundedNode = groundNode;
        ungroundedNode = airNode;
        this.bot = bot;
        b_isRunning = false;
        b_runningNormal = true;
    }

    public override NodeState Evaluate()
    {
        if (!b_isRunning)
        {
            b_runningNormal = !bot.isTurtled;
        }

        NodeState result = (b_runningNormal ? groundedNode.Evaluate() : ungroundedNode.Evaluate());
        b_isRunning = (result == NodeState.RUNNING);
        return result;
    }
}