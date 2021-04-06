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

    public float botTimer = 10f;

    private int attackBotMode;
    private int botSelection;

    bool isFacingUp = false;
    bool canFly = true;
    bool isParticlePlaying = false;
    bool botChoosen = false;

    public float fuel = 2.0f;
    public float rocketSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(botChoosen == false)
        {
            botSelection = Random.Range(0, 1);
            botChoosen = true;
        }
        if(botSelection == 0)
        {
            AttackBot();
        }
    }

    void AttackBot()
    {
        var botController = attackBot.GetComponent<BotBasic_Move>();
        var rb = attackBot.GetComponent<Rigidbody>();
        attackBotMode = Random.Range(0, 1);
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
                //fuel = 2.0f;
            }
            if(fuel == 2.0f)
            {
                canFly = true;
            }
           break;
           case 1:
                rb.AddForce(rb.centerOfMass - new Vector3(0, botController.boostSpeed * 50, 0), ForceMode.Impulse);
                attackBot.transform.rotation = Quaternion.Euler(90, 0, 0);
                shockWaveSpawner.SetActive(true);
                break;
           default:
               print("no attack choosen");
               break;
        }
    }
}
