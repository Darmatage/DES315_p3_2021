using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_Jump : B05_Node
{
    private float timer;
    private const float MAX_TIME = 1.8f;
    private bool b_running;

    protected Bot05_Move bot;

    public B05N_Jump(Bot05_Move bot)
    {
        this.bot = bot;
        b_running = false;
        timer = 0.0f;
    }

    public override NodeState Evaluate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > MAX_TIME)
        {
            timer = 0.0f;
            b_running = false;
            return NodeState.SUCCESS;
        }

        if (!b_running)
        {
            bot.Jump();
            b_running = true;
        }

        return NodeState.RUNNING;
    }
}
