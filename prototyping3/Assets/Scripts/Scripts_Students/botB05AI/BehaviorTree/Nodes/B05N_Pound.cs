using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_Pound : B05_UNode
{
    protected B05_GroundPound pound;
    protected Bot05_Move bot;
    protected B05_AI ai;
    private bool b_running;

    public B05N_Pound(B05_GroundPound pound, Bot05_Move bot, B05_AI ai)
    {
        this.pound = pound;
        this.bot = bot;
        this.ai = ai;
        b_running = false;
    }

    public override NodeState Evaluate()
    {
        if (!b_running)
        {
            ai.GetAgent().enabled = false;
            if (pound.ActivateAI())
            {
                b_running = true;
                Debug.Log("Ground Pound activated");
            }
            else
            {
                ai.GetAgent().enabled = true;
                return NodeState.FAILURE;
            }
        }
        else if (bot.IsState(Bot05_Move.STATE.NORMAL))
        {
            b_running = false;
            ai.GetAgent().enabled = true;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    public override float CalcScore()
    {
        switch(pound.NumOfTops())
        {
            case 0:
                return 0.0f;
            case 1:
                return 0.1f;
            case 2:
                return 0.3f;
            case 3:
                return 0.8f;
            case 4:
                return 1.0f;
            case 5:
                return 1.0f;
            default:
                return 0.0f;
        }
    }
}
