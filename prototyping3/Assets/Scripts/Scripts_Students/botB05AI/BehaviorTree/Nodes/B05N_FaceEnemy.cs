using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05N_FaceEnemy : B05_UNode
{
    private float MIN_DEGREE = 5.0f;
    private float MAX_TIME = 2.8f;

    private Transform enemy;
    private Bot05_Move bot;

    private float timer;

    public B05N_FaceEnemy (Transform enemy, Bot05_Move bot)
    {
        this.enemy = enemy;
        this.bot = bot;
        timer = 0.0f;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (timer > MAX_TIME)
        {
            bot.horizontal_movement = 0.0f;
            _nodeState = NodeState.FAILURE;
            timer = 0.0f;
            return _nodeState;
        }

        Vector2 goal = new Vector2(enemy.position.x - bot.transform.position.x, enemy.position.z - bot.transform.position.z);
        Vector2 facing = new Vector2(bot.transform.forward.x, bot.transform.forward.z);
        goal.Normalize();

        float angle = Vector2.SignedAngle(goal, facing);

        if (Mathf.Abs(angle) < MIN_DEGREE)
        {
            bot.horizontal_movement = 0.0f;
            _nodeState = NodeState.SUCCESS;
            timer = 0.0f;
            return _nodeState;
        }

        if (angle > 0.0f)
        {
            bot.horizontal_movement = 1.0f;
        }
        else
        {
            bot.horizontal_movement = -1.0f;
        }

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }

    public override float CalcScore()
    {
        return 0.0f;
    }
}
