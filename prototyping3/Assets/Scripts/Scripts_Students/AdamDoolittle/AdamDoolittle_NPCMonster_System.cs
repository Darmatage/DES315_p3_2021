using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamDoolittle_NPCMonster_System : MonoBehaviour
{
    public GameObject attackBot;
    public GameObject stunBot;
    public GameObject JetBooster1;
    public GameObject JetBooster2;
    public GameObject shockWaveSpawner;

    public Transform player1Target;
    public Transform player2Target;

    Vector3 stunBotStartPos;
    Vector3 stunBotCurPos;

    public float botTimer = 10f;
    public float attackBotTimer = 3.0f;
    public float attackSpeed = 1.0f;
    public float stunBotTimer = 3.0f;

    private int attackBotMode;
    private int botSelection;
    private int choosePlayer;


    bool isFacingUp = false;
    bool canFly = true;
    bool isParticlePlaying = false;
    bool botChoosen = false;
    bool attackBotChoosen = false;
    bool stunBotChoosen = false;
    bool isFuelCharging = false;
    bool isAttackBotDone = false;
    bool isStunBotDone = false;
    bool attackPlayer1 = false;
    bool attackPlayer2 = false;
    bool loadPlayersHasBeenCalled = false;

    public float fuel = 2.0f;
    public float rocketSpeed = 10.0f;

    private NPC_LoadPlayers playerLoader;

    // Start is called before the first frame update
    void Start()
    {
        stunBotStartPos = new Vector3(stunBot.transform.position.x, stunBot.transform.position.y, stunBot.transform.position.z);
        playerLoader = GetComponent<NPC_LoadPlayers>();
    }

    // Update is called once per frame
    void Update()
    {
        //stunBotCurPos = new Vector3(stunBot.transform.position.x, stunBot.transform.position.y, stunBot.transform.position.z);
        if(isStunBotDone == true)
        {
            isStunBotDone = false;
            botChoosen = false;
            stunBotChoosen = false;
        }
        if(isAttackBotDone == true)
        {
            isAttackBotDone = false;
            botChoosen = false;
            attackBotChoosen = false;
        }
        if(botChoosen == false)
        {
            botSelection = Random.Range(0, 2);
            botChoosen = true;
        }
        if(botSelection == 0)
        {
            if(attackBotChoosen == false)
            {
                AttackBotAbilityChoose();
            }
            AttackBot();
            attackBotChoosen = true;
        }
        if(botSelection == 1)
        {
            //botSelection = Random.Range(0, 2);
            if(stunBotChoosen == false)
            {
                StunBotChoosePlayer();
            }
            StunBot();
            stunBotChoosen = true;
        }

        if(playerLoader.playersReady == true)
        {
            LoadPlayerTargets();
        }
    }

    void AttackBot()
    {
        var botController = attackBot.GetComponent<BotBasic_Move>();
        var rb = attackBot.GetComponent<Rigidbody>();
        //attackBotMode = Random.Range(0, 2);
        switch(attackBotMode)
        {
            case 0:
            if (canFly == true)
            {
                rb.AddForce(rb.centerOfMass + new Vector3(0f, botController.jumpSpeed * rocketSpeed, 0f), ForceMode.Force);
                fuel -= Time.deltaTime;
                //Debug.Log(fuel);
                if (isFacingUp == false)
                {
                    //transform.Rotate(-90, 0, 0);
                    attackBot.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    isFacingUp = true;
                }
                if(isFacingUp == true)
                {
                    attackBot.transform.rotation = Quaternion.Euler(-90, 0, 0);
                }
                if (isParticlePlaying == false)
                {
                    JetBooster1.GetComponent<ParticleSystem>().Play();
                    JetBooster2.GetComponent<ParticleSystem>().Play();
                    isParticlePlaying = true;
                }
            }
            if (fuel <= 0.0f)
            {
                canFly = false;
                JetBooster1.GetComponent<ParticleSystem>().Stop();
                JetBooster2.GetComponent<ParticleSystem>().Stop();
                isParticlePlaying = false;
                isFuelCharging = true;
                //fuel = 2.0f;
            }
            if(isFuelCharging == true)
            {
                canFly = true;
                fuel = 2.0f;
                isAttackBotDone = true;
                isFuelCharging = false;
            }
           break;
           case 1:
                if (attackBotTimer == 3.0f)
                {
                    rb.AddForce(rb.centerOfMass - new Vector3(0, botController.boostSpeed * 50, 0), ForceMode.Impulse);
                    attackBot.transform.rotation = Quaternion.Euler(90, 0, 0);
                    isFacingUp = false;
                    shockWaveSpawner.SetActive(true);
                }
                if(attackBotTimer <= 0.0f && isAttackBotDone == false)
                {
                    isAttackBotDone = true;
                    attackBotTimer = 3.0f;
                }
                else
                {
                    attackBotTimer -= Time.deltaTime;
                }
                break;
            
           default:
               print("no attack choosen");
               break;
        }
    }

    void AttackBotAbilityChoose()
    {
        if(attackBotMode == 0)
        {
            attackBotMode = 1;
        }
        else
        {
            attackBotMode = 0;
        }
        //attackBotMode = Random.Range(0, 2);
    }

    void StunBot()
    {
        var rb = stunBot.GetComponent<Rigidbody>();
        var botController = stunBot.GetComponent<BotBasic_Move>();
        switch(choosePlayer)
        {
            case 0:
                if(stunBotTimer <= 3.0f && stunBotTimer > 0.0f)
                {
                    if(loadPlayersHasBeenCalled == true)
                    {
                        attackPlayer1 = true;
                        stunBot.transform.LookAt(player1Target);
                        //stunBot.transform.position = Vector3.Lerp(stunBot.transform.position, player1Target.position, Time.deltaTime * attackSpeed);
                        //stunBot.transform.position = Vector3.MoveTowards(stunBot.transform.position, player1Target.position, Time.deltaTime * attackSpeed);
                        //rb.AddForceAtPosition(rb.centerOfMass - new Vector3(0, 0, botController.boostSpeed + 10), player1Target.position, ForceMode.Impulse);
                        //rb.MovePosition(player1Target.position);
                        rb.AddRelativeForce(0, 0, 1000);
                        //rb.AddForce(player1Target.position * attackSpeed);
                    }
                }
                if(stunBotTimer <= 0.0f)
                {
                    //Debug.Log(isStunBotDone.ToString());
                    if (isStunBotDone == false)
                    {
                        //stunBot.transform.position = Vector3.Lerp(stunBot.transform.position, stunBotStartPos, attackSpeed);
                        //Vector3.MoveTowards(stunBot.transform.position, stunBotStartPos, attackSpeed);
                        //stunBot.transform.position = stunBotStartPos;
                        //if (stunBot.transform.position == stunBotStartPos)
                        //{
                            isStunBotDone = true;
                            attackPlayer1 = false;
                            stunBotTimer = 3.0f;
                        //}
                    }
                }
                else if(loadPlayersHasBeenCalled == true)
                {
                    stunBotTimer -= Time.deltaTime;
                }
                break;

            case 1:
                if (stunBotTimer <= 3.0f && stunBotTimer > 0.0f)
                {
                    if (loadPlayersHasBeenCalled == true)
                    {
                        attackPlayer2 = true;
                        stunBot.transform.LookAt(player2Target);
                        //stunBot.transform.position = Vector3.Lerp(stunBot.transform.position, player2Target.position, Time.deltaTime * attackSpeed);
                        //stunBot.transform.position = Vector3.MoveTowards(stunBot.transform.position, player2Target.position, Time.deltaTime * attackSpeed);
                        //rb.AddForceAtPosition(rb.centerOfMass - new Vector3(0, 0, botController.boostSpeed + 10), player2Target.position, ForceMode.Impulse);
                        //rb.MovePosition(player2Target.position);
                        rb.AddRelativeForce(0, 0, 1000);
                        //rb.AddForce(player2Target.position * attackSpeed);
                    }
                }
                if (stunBotTimer <= 0.0f)
                {
                    if (isStunBotDone == false)
                    {
                        //stunBot.transform.position = Vector3.Lerp(stunBot.transform.position, stunBotStartPos, Time.deltaTime * attackSpeed);
                        //Vector3.MoveTowards(stunBot.transform.position, stunBotStartPos, attackSpeed);
                        //stunBot.transform.position = stunBotStartPos;
                        //if (stunBot.transform.position == stunBotStartPos)
                        //{
                            isStunBotDone = true;
                            attackPlayer2 = false;
                            stunBotTimer = 3.0f;
                        //}
                    }
                }
                else if(loadPlayersHasBeenCalled == true)
                {
                    stunBotTimer -= Time.deltaTime;
                }
                break;

            default:
                print("No player Choosen");
                break;
        }
        //choosePlayer = Random.Range(0, 2);
        //if (choosePlayer == 0 && isStunBotDone == false)
        //{
        //    if (stunBotTimer > 0.0f)
        //    {
        //        if (loadPlayersHasBeenCalled == true)
        //        {
        //            attackPlayer1 = true;
        //            stunBot.transform.LookAt(player1Target);
        //            Vector3.Lerp(stunBotStartPos, player1Target.position, attackSpeed);
        //        }
        //    }
        //    if (stunBotTimer <= 0.0f)
        //    {
        //        //isStunBotDone = true;

        //        Vector3.Lerp(stunBotCurPos, stunBotStartPos, attackSpeed);
        //        if (stunBotCurPos == stunBotStartPos)
        //        {
        //            isStunBotDone = true;
        //            attackPlayer1 = false;
        //            stunBotTimer = 3.0f;
        //        }
        //    }
        //    else
        //    {
        //        stunBotTimer -= Time.deltaTime;
        //    }
        //}
        //if (choosePlayer == 1 && isStunBotDone == false)
        //{
        //    if (stunBotTimer > 0.0f)
        //    {
        //        if (loadPlayersHasBeenCalled == true)
        //        {
        //            attackPlayer2 = true;
        //            stunBot.transform.LookAt(player2Target);
        //            Vector3.Lerp(stunBotStartPos, player2Target.position, attackSpeed);
        //        }
        //    }
        //    if (stunBotTimer <= 0.0f)
        //    {
        //        //isStunBotDone = true;

        //        Vector3.Lerp(stunBotCurPos, stunBotStartPos, attackSpeed);
        //        if (stunBotCurPos == stunBotStartPos)
        //        {
        //            isStunBotDone = true;
        //            attackPlayer2 = false;
        //            stunBotTimer = 3.0f;
        //        }
        //    }
        //    else
        //    {
        //        stunBotTimer -= Time.deltaTime;
        //    }
        //}
    }

    void StunBotChoosePlayer()
    {
        choosePlayer = Random.Range(0, 2);
    }

    public void LoadPlayerTargets()
    {
        loadPlayersHasBeenCalled = true;
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

    
}
