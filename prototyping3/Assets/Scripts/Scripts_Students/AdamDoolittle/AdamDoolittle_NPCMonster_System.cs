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

    public float botTimer = 10f;
    public float attackBotTimer = 3.0f;

    private int attackBotMode;
    private int botSelection;

    bool isFacingUp = false;
    bool canFly = true;
    bool isParticlePlaying = false;
    bool botChoosen = false;
    bool attackBotChoosen = false;
    bool stunBotChoosen = false;
    bool isFuelCharging = false;
    bool isAttackBotDone = false;

    public float fuel = 2.0f;
    public float rocketSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            botSelection = Random.Range(0, 2);
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
}
