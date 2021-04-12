using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LJN_MonsterScript : MonoBehaviour
{
    private NavMeshAgent myAgent;
    public Transform player1Target;
    public Transform player2Target;
    public Transform defaultPatrolTarget;
    public Transform nextPatrolTarget;

    public float playerAttackDistance = 1f;
    public float turnThreshold = 40f;
    public float distToPlayer1;
    public float distToPlayer2;
    public float distToTarget;
    public bool attackPlayer1 = false;
    public bool attackPlayer2 = false;
    public bool getNextTarget = true;

    private NPC_LoadPlayers playerLoader;

    public Transform attackPoint;
    public float attackRange = 2f;

    public bool isAttacking = false;
    private float attackTimer = 0;
    public float attackRate = 0.2f;

    public GameObject tail;
    public GameObject rightArm;
    public GameObject leftArm;

    public float ArmSpeed = 5.0f;
    public float SawSpeed = 5.0f;

    private float ArmLerpAmountLeft = 0;
    private float ArmLerpAmountRight = 0;
    private float SawLerpAmount = 0;

    public Vector3 LeftGrabArmStart;
    public Vector3 RightGrabArmStart;
    public Vector3 SawArmStart;

    public Vector3 LeftGrabArmEnd;
    public Vector3 RightGrabArmEnd;
    public Vector3 SawArmEnd;

    private float distThreshold = 12f;
    private float spinTimer;
 

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        playerLoader = GetComponent<NPC_LoadPlayers>();
        nextPatrolTarget = defaultPatrolTarget;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        distToTarget = Vector3.Distance(nextPatrolTarget.position, gameObject.transform.position);

        if ((player1Target != null) && (player2Target != null))
        {
            //get distances to players 		
            distToPlayer1 = Vector3.Distance(player1Target.position, gameObject.transform.position);
            distToPlayer2 = Vector3.Distance(player2Target.position, gameObject.transform.position);

            //is the player close enough to attack?
            if ((distToPlayer1 <= playerAttackDistance) && (distToPlayer1 < distToPlayer2))
            {
                attackPlayer1 = true;
            }
            else
            {
                attackPlayer1 = false;
            }

            if ((distToPlayer2 <= playerAttackDistance) && (distToPlayer2 < distToPlayer1))
            {
                attackPlayer2 = true;
            }
            else
            {
                attackPlayer2 = false;
            }
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

        //if (distToTarget <= patrolSwitchThreshold1)
        //{
        //    if (getNextTarget == true)
        //    {
        //        StopCoroutine(FindNextPatrolTarget());
        //        StartCoroutine(FindNextPatrolTarget());
        //    }
        //    getNextTarget = false;
        //}
        //else if (distToTarget >= patrolSwitchThreshold2)
        //{
        //    getNextTarget = true;
        //}

        if (playerLoader.playersReady == true)
        {
            LoadPlayerTargets();

            if (distToTarget <= playerAttackDistance)
            {
                ChooseTarget();
               
            }
        }

      

        if(!isAttacking)
        {
            SawLerpAmount -= dt * SawSpeed;

            if (SawLerpAmount < 0.0f) SawLerpAmount = 0.0f;

            tail.transform.localEulerAngles = Vector3.Lerp(SawArmStart, SawArmEnd, SawLerpAmount);
        }
    }

    public void AttackPlayer()
    {
        float dt = Time.deltaTime;
        //use attack pattern

        SawLerpAmount += dt * SawSpeed;

        if (SawLerpAmount > 1.0f) SawLerpAmount = 1.0f;
        tail.transform.localEulerAngles = Vector3.Lerp(SawArmStart, SawArmEnd, SawLerpAmount);


        attackTimer += dt;
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

    public void OnCollisionEnter(Collision other)
    {
        Transform teleportBelow = GameObject.FindWithTag("FallRespawn").transform;

        if ((player1Target != null) && (player2Target != null))
        {
            if (other.gameObject.transform.parent != null)
            {
                if (other.gameObject.transform.parent.tag == "Player1")
                {
                    if (GameHandler.p1Health <= 0)
                    {
                        player1Target.position = teleportBelow.position;
                        Debug.Log("I am teleporting " + GameHandler.player1Prefab + " (Player 1) to " + teleportBelow.position);
                    }
                }

                if (other.gameObject.transform.parent.tag == "Player2")
                {
                    if (GameHandler.p2Health <= 0)
                    {
                        player2Target.position = teleportBelow.position;
                        Debug.Log("I am teleporting " + GameHandler.player2Prefab + " (Player 2) to " + teleportBelow.position);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("ROOT: " + other.name);
    }

    private void ChooseTarget()
    {
        //based on attack pattern
        if(attackTimer >= 2.5f)
        {
            nextPatrolTarget = player1Target;
            attackTimer = 0;
        }
        else
        {
            myAgent.destination = gameObject.transform.position;
            Debug.Log("wait");
        }
    }

}
