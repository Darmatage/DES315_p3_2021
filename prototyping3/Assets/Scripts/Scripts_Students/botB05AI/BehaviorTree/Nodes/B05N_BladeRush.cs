using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_BladeRush : B05_UNode
{
    private float IDEAL_DIST = 5.0f;
    private float BAD_DIST = 30.0f;

    private B05_BladeRush rush;
    private Bot05_Move bot;
    private Transform enemy;
    private B05_AI ai;
    private bool b_running;

    public B05N_BladeRush (B05_BladeRush rush, Bot05_Move bot, Transform enemy, B05_AI ai)
    {
        this.rush = rush;
        this.bot = bot;
        this.enemy = enemy;
        this.ai = ai;
        b_running = false;
    }

    public override NodeState Evaluate()
    {
        ai.mesh.material = ai.matrush;

        if (!b_running)
        {
            rush.Attack();
            b_running = true;
            //Debug.Log("Blade Rush activated.");
        }
        else if (bot.IsState(Bot05_Move.STATE.NORMAL))
        {
            b_running = false;
            return NodeState.SUCCESS;
        }
        
        return NodeState.RUNNING;
    }

    public bool IsAvaliable()
    {
        return rush.IsAvaliable();
    }

    public override float CalcScore()
    {

        if (!IsAvaliable())
            return 0.0f;

        float dist = Vector3.Distance(enemy.position, ai.transform.position);

        if (dist <= IDEAL_DIST)
        {
            return (ai.low_health ? 0.6f : 1.0f);
        }
        else
        {
            return Mathf.Clamp(1 - (dist / BAD_DIST), 0.0f, 1.0f);
        }
    }
}
