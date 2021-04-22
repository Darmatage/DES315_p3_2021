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

    private int attackChoice;
    private int attackStage = 200;

    public Vector3 EndRot;

    private Vector3 currentEnd ;
 

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        playerLoader = GetComponent<NPC_LoadPlayers>();
        nextPatrolTarget = defaultPatrolTarget;
        currentEnd = SawArmEnd;
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
            //if ((distToPlayer1 <= playerAttackDistance) && (distToPlayer1 < distToPlayer2))
            //{
            //    attackPlayer1 = true;
            //}
            //else
            //{
            //    attackPlayer1 = false;
            //}

            //if ((distToPlayer2 <= playerAttackDistance) && (distToPlayer2 < distToPlayer1))
            //{
            //    attackPlayer2 = true;
            //}
            //else
            //{
            //    attackPlayer2 = false;
            //}
        }

        //am I moving towards the player or my next patrol location? 
        //if (attackPlayer1 == true)
        //{
        //    myAgent.destination = player1Target.position;
        //    if (distToPlayer1 > turnThreshold && attackChoice == 0 && attackStage ==0)
        //    {
        //        transform.LookAt(player1Target);
        //    }
        //    isAttacking = true;
        //    AttackPlayer();
        //}
        //else if (attackPlayer2 == true)
        //{
        //    myAgent.destination = player2Target.position;
        //    if (distToPlayer2 > turnThreshold && attackChoice == 0 && attackStage == 0)
        //    {
        //        transform.LookAt(player2Target);
        //    }
        //    isAttacking = true;
        //    AttackPlayer();
        //}
        //else
        if(distToTarget <= playerAttackDistance && attackStage !=200)
        {
           // myAgent.destination = nextPatrolTarget.position;
            if (attackChoice == 0)
            {
                if (attackStage == 0)
                {
                    if (distToTarget > turnThreshold )
                    {
                        transform.LookAt(nextPatrolTarget);
                    }
                    attackStage = 1;
                }
                isAttacking = true;

            }
            else if (attackChoice == 1)
            {
                if (attackStage == 0)
                {
                    attackStage = 1;
                    isAttacking = true;
                    myAgent.destination = nextPatrolTarget.position;
                }
            }
            else
            {
                if (attackStage == 0)
                {
                    if (distToTarget > turnThreshold)
                    {
                        transform.LookAt(nextPatrolTarget);
                    }
                    attackStage = 1;
                }
               
            }
            AttackPlayer();
        }
        else if(distToTarget > playerAttackDistance)
        {
            myAgent.destination = nextPatrolTarget.position;
            if(attackChoice == 2)
            {
                AttackPlayer();
                
            }
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

            if (attackStage == 200)//distToTarget <= playerAttackDistance
            {
                ChooseTarget();
               
            }
        }

      

        if(!isAttacking)
        {
            SawLerpAmount -= dt * SawSpeed;

            if (SawLerpAmount < 0.0f) SawLerpAmount = 0.0f;

            tail.transform.localEulerAngles = Vector3.Lerp(SawArmStart, currentEnd, SawLerpAmount);
        }
    }

    public void AttackPlayer()
    {
        float dt = Time.deltaTime;
        //use attack pattern

        if (attackChoice == 0 && attackStage == 1)
        {
            SawLerpAmount += dt * SawSpeed;

            if (SawLerpAmount > 1.0f) SawLerpAmount = 1.0f;
            tail.transform.localEulerAngles = Vector3.Lerp(SawArmStart, SawArmEnd, SawLerpAmount);

            if(attackTimer >= 1.5f)
            {
                attackStage = 200;
                isAttacking = false;
            }
        }

        if(attackChoice == 1)
        {

            if (attackStage != 2)
            {
                SawLerpAmount += dt * SawSpeed;

                if (SawLerpAmount > 1.0f)
                    SawLerpAmount = 1.0f;
                tail.transform.localEulerAngles = Vector3.Lerp(SawArmStart, EndRot, SawLerpAmount);
            }
            else
            {
                if (SawLerpAmount == 0.0f)
                {
                    attackStage = 200;
                }
            }

            if (SawLerpAmount == 1.0f && attackStage == 0)
            {
                attackStage = 1;
                attackTimer = 0;
            }

            if(attackStage == 1)
            {
                transform.Rotate(new Vector3(0, 420 * dt,0));
               // Debug.Log(tail.GetComponent<HazardDamage>().damage);

                if(attackTimer >= 4)
                {
                    attackStage = 2;
                    isAttacking = false;
                }
            }

        }

        if(attackChoice == 2)
        {
            if (attackStage == 0)
            {
                ArmLerpAmountLeft += dt * ArmSpeed;
                ArmLerpAmountRight += dt * ArmSpeed;


                if (ArmLerpAmountLeft > 1.0f && ArmLerpAmountRight > 1.0f)
                {
                    ArmLerpAmountLeft = 1.0f;
                    ArmLerpAmountRight = 1.0f;
                    if(distToTarget < playerAttackDistance)
                        attackStage = 1;

                }

                rightArm.transform.localEulerAngles = Vector3.Lerp(RightGrabArmStart, RightGrabArmEnd, ArmLerpAmountRight);
                leftArm.transform.localEulerAngles = Vector3.Lerp(LeftGrabArmStart, LeftGrabArmEnd, ArmLerpAmountLeft);

                
            }
            else if (attackStage == 1)
            {
                ArmLerpAmountLeft -= dt * ArmSpeed;
                ArmLerpAmountRight -= dt * ArmSpeed;


                if (ArmLerpAmountLeft < 0.0f && ArmLerpAmountRight < 0.0f)
                {
                    ArmLerpAmountLeft = 0.0f;
                    ArmLerpAmountRight = 0.0f;
                    attackStage = 200;

                }

                rightArm.transform.localEulerAngles = Vector3.Lerp(RightGrabArmStart, RightGrabArmEnd, ArmLerpAmountRight);
                leftArm.transform.localEulerAngles = Vector3.Lerp(LeftGrabArmStart, LeftGrabArmEnd, ArmLerpAmountLeft);
            }
           
        }

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
        if(attackStage == 200)
        {
            attackStage = 0;
            myAgent.speed = 30;
            if (attackChoice == 1)
            {
                tail.GetComponent<HazardDamage>().damage = 0;
                attackChoice = 0 + (Random.Range(0, 2) * 2);
            }
            else
            {
                attackChoice = Random.Range(0, 2);

            }

            //attackChoice = 1;

            tail.transform.localPosition = new Vector3(0f, 0.5f, -2.01f);
            switch (attackChoice)
            {
                case 0: //charge
                    if(Random.Range(1,3) == 1)
                    {
                        nextPatrolTarget = player1Target;
                    }
                    else
                    {
                        nextPatrolTarget = player2Target;
                    }
                    currentEnd = SawArmEnd;
                    break;
                case 1: //spin
                    tail.GetComponent<HazardDamage>().damage = 10;
                    tail.transform.localPosition = new Vector3(0, 0, -2.01f);
                    currentEnd = EndRot;
                    nextPatrolTarget = transform;
                    break;
                case 2: //block and push
                    myAgent.speed = 60;
                   
                    if (Random.Range(1, 3) == 1)
                    {
                        nextPatrolTarget = player1Target;
                    }
                    else
                    {
                        nextPatrolTarget = player2Target;
                    }
                    break;
            }
           
            attackTimer = 0;
        }
        else
        {
            myAgent.destination = gameObject.transform.position;
            Debug.Log("wait");
        }
    }

}
