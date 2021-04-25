using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JWymer_NPC_Rotation : MonoBehaviour
{
    public JWymer_NPC_Patrol_Strafe patrolStrafe;
    public bool targetingPlayer1 = true; // false for Player 2

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Transform target;

        if (targetingPlayer1)
		{
            if (patrolStrafe.attackPlayer1)
            {
                target = patrolStrafe.player1Target;
            }
            else
                return;
		}
        else
		{
            if (patrolStrafe.attackPlayer2)
            {
                target = patrolStrafe.player2Target;
            }
            else
                return;
		}

        transform.LookAt(target);
    }
}
