using System.Collections;
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

    private GameHandler gameHandler;

    enum WreckingBallState
    {
        Swinging,
        Raise,
        Smash,
        Stalled
    }

    private WreckingBallState state = WreckingBallState.Swinging;
    private WreckingBallState prevState = WreckingBallState.Swinging;

    float trigTimer = 0;
    float stalledTimer = 0;
    float stalledTimerMax = 2.5f;
    Vector3 oldBallPos;
    Vector3 targetPos;

    float oldSpeed;

    void Start()
    {
        if (GameObject.FindWithTag("GameHandler") != null)
        {
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }

        myAgent = GetComponent<NavMeshAgent>();
        playerLoader = GetComponent<NPC_LoadPlayers>();

        nextPatrolTarget = patrolTarget1;

        oldSpeed = myAgent.speed;
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
            attackPlayer1 = (distToPlayer1 <= playerAttackDistance) && (distToPlayer1 < distToPlayer2);
            attackPlayer2 = (distToPlayer2 <= playerAttackDistance) && (distToPlayer2 < distToPlayer1);
        }

        //am I moving towards the player or my next patrol location? 
        if (attackPlayer1 == true)
        {
            myAgent.destination = player1Target.position;
            if (distToPlayer1 > turnThreshold)
            {
                transform.LookAt(player1Target);
            }
            if (distToPlayer1 < 15f && state == WreckingBallState.Swinging)
            {
                state = WreckingBallState.Raise;
                myAgent.speed = 0;
            }
        }
        else if (attackPlayer2 == true)
        {
            myAgent.destination = player2Target.position;
            if (distToPlayer2 > turnThreshold)
            {
                transform.LookAt(player2Target);
            }
            if (distToPlayer2 < 15f && state == WreckingBallState.Swinging)
            {
                state = WreckingBallState.Raise;
                myAgent.speed = 0;
            }
        }
        else
        {
            myAgent.destination = nextPatrolTarget.position;
            if (distToTarget > turnThreshold)
            {
                transform.LookAt(nextPatrolTarget);
            }
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

        if (other.gameObject.tag == "Hazard")
        {
            float attackDamage = other.gameObject.GetComponent<HazardDamage>().damage;

            gameHandler.TakeDamage("CoopNPCMonster", attackDamage);
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

    float timer = 0;

    private void UpdateWreckingBall()
    {
        bool stateChanged = (state != prevState);
        timer += Time.deltaTime;

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
            if (stateChanged)
            {
                timer = 0;

                float swingSin = Mathf.Sin(trigTimer);
                float swingCos = Mathf.Cos(trigTimer);

                Vector3 newPos = new Vector3();
                newPos.x = swingSin * 14f;
                newPos.z = swingCos * 14f;

                oldBallPos = transform.position + newPos;
            }

            Vector3 above = transform.position;
            above.y += 14f;

            wreckingBall.position = Vector3.MoveTowards(wreckingBall.position, above, timer / 2f);

            if (timer >= 1.0f)
            {
                state = WreckingBallState.Smash;
                oldBallPos = above;
                return;
            }
        }
        else if (state == WreckingBallState.Smash)
        {
            if (stateChanged)
            {
                timer = 0;

                if (attackPlayer1 == true)
                {
                    targetPos = player1Target.transform.position;
                }
                else if (attackPlayer2 == true)
                {
                    targetPos = player2Target.transform.position;
                }
            }

            wreckingBall.position = Vector3.MoveTowards(wreckingBall.position, targetPos, timer);

            if (timer >= 1.0f)
            {
                state = WreckingBallState.Stalled;
                return;
            }
        }
        else if (state == WreckingBallState.Stalled)
        {
            stalledTimer += Time.deltaTime;

            if (stalledTimer >= stalledTimerMax)
            {
                stalledTimer = 0;
                state = WreckingBallState.Swinging;
                myAgent.speed = oldSpeed;
                return;
            }
        }

        prevState = state;
    }
}