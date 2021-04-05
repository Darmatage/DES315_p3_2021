﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Modified from Jason Wisers code for NPC_PatrolAndAttack

class Mars_WreckingBall : MonoBehaviour
{
    private NavMeshAgent myAgent;
    public Transform player1Target;
    public Transform player2Target;
    public Transform nextPatrolTarget;

    public Transform patrolTarget1;
    public Transform patrolTarget2;
    public Transform patrolTarget3;
    public Transform patrolTarget4;

    public float playerAttackDistance = 25f;
    public float patrolSwitchThreshold1 = 8f;
    public float patrolSwitchThreshold2 = 30f;
    public float turnThreshold = 40f;
    public float distToPlayer1;
    public float distToPlayer2;
    public float distToTarget;
    public bool attackPlayer1 = false;
    public bool attackPlayer2 = false;
    public bool getNextTarget = true;

    private NPC_LoadPlayers playerLoader;

    public Transform wreckingBall;
    public bool isAttacking = false;

    enum WreckingBallState
    {
        Swinging,
        Raise,
        Smash,
        Stalled
    }

    private WreckingBallState state = WreckingBallState.Swinging;

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        playerLoader = GetComponent<NPC_LoadPlayers>();

        nextPatrolTarget = patrolTarget1;
    }

    private void Update()
    {
        //get distances to next target 
        distToTarget = Vector3.Distance(nextPatrolTarget.position, gameObject.transform.position);

        if ((player1Target != null) && (player2Target != null))
        {
            //get distances to players 		
            distToPlayer1 = Vector3.Distance(player1Target.position, gameObject.transform.position);
            distToPlayer2 = Vector3.Distance(player2Target.position, gameObject.transform.position);

            //is the player close enough to attack?
            attackPlayer1 = ((distToPlayer1 <= playerAttackDistance) && (distToPlayer1 < distToPlayer2));
            attackPlayer2 = ((distToPlayer2 <= playerAttackDistance) && (distToPlayer2 < distToPlayer1));
        }

        //am I moving towards the player or my next patrol location? 
        if (attackPlayer1 == true)
        {
            myAgent.destination = player1Target.position;
            if (distToPlayer1 > turnThreshold)
            {
                transform.LookAt(player1Target);
            }
            isAttacking = true;
            AttackPlayer();
        }
        else if (attackPlayer2 == true)
        {
            myAgent.destination = player2Target.position;
            if (distToPlayer2 > turnThreshold)
            {
                transform.LookAt(player2Target);
            }
            isAttacking = true;
            AttackPlayer();
        }
        else
        {
            myAgent.destination = nextPatrolTarget.position;
            if (distToTarget > turnThreshold)
            {
                transform.LookAt(nextPatrolTarget);
            }
            isAttacking = false;
            AttackPlayer();
        }

        //have a I reached a patrol destination, so I should switch to the next?
        if (distToTarget <= patrolSwitchThreshold1)
        {
            if (getNextTarget == true)
            {
                StopCoroutine(FindNextPatrolTarget());
                StartCoroutine(FindNextPatrolTarget());
            }
            getNextTarget = false;
        }
        else if (distToTarget >= patrolSwitchThreshold2)
        {
            getNextTarget = true;
        }

        if (playerLoader.playersReady == true)
        {
            LoadPlayerTargets();
        }

        UpdateWreckingBall();
    }

    public void OnCollisionEnter(Collision other)
    {
        Transform teleportBelow = GameObject.FindWithTag("FallRespawn").transform;

        if ((player1Target != null) && (player2Target != null))
        {
            if (other.gameObject.transform.parent != null)
            {
                if (other.gameObject.transform.parent.tag == "Player1" && GameHandler.p1Health <= 0)
                {
                    player1Target.position = teleportBelow.position;
                    Debug.Log("I am teleporting " + GameHandler.player1Prefab + " (Player 1) to " + teleportBelow.position);
                }

                if (other.gameObject.transform.parent.tag == "Player2" && GameHandler.p2Health <= 0)
                {
                    player2Target.position = teleportBelow.position;
                    Debug.Log("I am teleporting " + GameHandler.player2Prefab + " (Player 2) to " + teleportBelow.position);
                }
            }
        }
    }

    IEnumerator FindNextPatrolTarget()
    {
        yield return new WaitForSeconds(0.1f); //pause at destination
        Debug.Log("Current at " + nextPatrolTarget + ". Getting next target.");
        if (nextPatrolTarget == patrolTarget1) { nextPatrolTarget = patrolTarget2; }
        else if (nextPatrolTarget == patrolTarget2) { nextPatrolTarget = patrolTarget3; }
        else if (nextPatrolTarget == patrolTarget3) { nextPatrolTarget = patrolTarget4; }
        else if (nextPatrolTarget == patrolTarget4) { nextPatrolTarget = patrolTarget1; }
    }

    public void LoadPlayerTargets()
    {
        //load players as targets when they appear:
        if (GameObject.FindWithTag("Player1").transform.GetChild(0) != null)
        {
            player1Target = GameObject.FindWithTag("Player1").transform.GetChild(0).GetComponent<Transform>();
        }
        if (GameObject.FindWithTag("Player2").transform.GetChild(0) != null)
        {
            player2Target = GameObject.FindWithTag("Player2").transform.GetChild(0).GetComponent<Transform>();
        }
    }

    public void AttackPlayer()
    {
        if (isAttacking == true)
        {
            
        }
        else
        {
            
        }
    }

    float trigTimer = 0;
    float stalledTimer = 0;
    float stalledTimerMax = 5f;

    private void UpdateWreckingBall()
    {
        if (state == WreckingBallState.Swinging)
        {
            trigTimer += Time.deltaTime * 2f;

            float swingSin = Mathf.Sin(trigTimer);
            float swingCos = Mathf.Cos(trigTimer);
            
            Vector3 newPos = new Vector3();
            newPos.x = swingSin * 14f;
            newPos.z = swingCos * 14f;

            wreckingBall.position = transform.position + newPos;
        }
        else if (state == WreckingBallState.Raise)
        {

        }
        else if (state == WreckingBallState.Smash)
        {

        }
        else if (state == WreckingBallState.Stalled)
        {
            stalledTimer += Time.deltaTime;

            if (stalledTimer >= stalledTimerMax)
            {
                stalledTimer = 0;
                state = WreckingBallState.Swinging;
            }
        }
    }
}