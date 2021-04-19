using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_Idle : B05_UNode
{
    private float TIME_MAX = 1.8f;
    private float TIME_MIN = 0.5f;
    private float goal = 0.0f;
    private float timer = 0.0f;

    private float SCORE = 0.2f;

    private bool b_just_idled = false;

    private Bot05_Move bot;

    public B05N_Idle(Bot05_Move bot)
    {
        this.bot = bot;
    }

    // stay still until time is up
    public override NodeState Evaluate()
    {
        if (goal == 0.0f)
        {
            goal = Random.Range(TIME_MIN, TIME_MAX);
            Debug.Log("Idling...");
        }

        timer += Time.deltaTime;
        if (timer > goal || !bot.isGrounded)
        {
            timer = goal = 0.0f;
            b_just_idled = true;
            _nodeState = NodeState.SUCCESS;
            return _nodeState;
        }

        return NodeState.RUNNING;
    }

    // score is zero after the bot has idled so that it never idles twice
    public override float CalcScore()
    {
        if (b_just_idled)
        {
            b_just_idled = false;
            return 0.001f;
        }
        else
        {
            return SCORE;
        }
    }
}
