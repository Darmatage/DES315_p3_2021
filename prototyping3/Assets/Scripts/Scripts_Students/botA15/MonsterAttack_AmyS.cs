using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack_AmyS : MonoBehaviour
{
    int num_shields_broken = 0;

    public GameObject rightShield;
    public GameObject bodyShield;
    public GameObject leftShield;
    public GameObject distanceCheck;
    public GameObject body;
    public GameObject rippleParticles;
    public GameObject blastCircleSprite;

    public float blastForceScalar = 100f;

    private MeshRenderer meshRend;
    private Color originalColor;

    private GameObject player1;
    private GameObject player2;
    private Rigidbody player1RB;
    private Rigidbody player2RB;

    public float blastRadius = 16f;

    bool isChargingBlast = false;

    public static float strobeDelay = .3f;
    float strobeDelayTimer = strobeDelay;
    bool toggle = false;
    float detonateTimer = 2.5f; // in seconds
    bool doOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        rightShield.SetActive(true);
        bodyShield.SetActive(true);
        leftShield.SetActive(true);

        num_shields_broken = 0;

        meshRend = body.GetComponent<MeshRenderer>();
        originalColor = meshRend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayers(); // gets the players

        if (player1 && player2)
        {
            BlastDefense();
        }
        

        
    }

    void BlastDefense()
    {
         // if one of the players is within blast range
        if (Vector3.Distance(player1.transform.position, distanceCheck.transform.position) <= blastRadius
               || Vector3.Distance(player2.transform.position, distanceCheck.transform.position) <= blastRadius)
        {
             // if it hasn't already been activated, initiate blast sequence
            if (!isChargingBlast)
                StartCoroutine(Blast());
        }
         // if they go outside the blast radius during counting
        else
        {
             // then if it hasn't already initiated, stop sequence
            if (!isChargingBlast)
                StopCoroutine(Blast());
        }

         // once the blast has been initiated
        if (isChargingBlast)
        {
             // strobe for a bit
            if (detonateTimer >= 0)
            {
                //Debug.Log("I am in the first if statement");
                Strobe();
                detonateTimer -= Time.deltaTime;
            }
             // once charge timer is up
            else
            {
                meshRend.material.SetColor("_Color", originalColor);
                blastCircleSprite.GetComponent<SpriteRenderer>().enabled = false;


                Vector3 newTrans = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (body.transform.localScale.y / 2.0f) +1f, transform.position.z);

                Vector3 newRotation = new Vector3(-90f, 0f, -0f);

                if(!doOnce)
                {
                    Instantiate(rippleParticles, newTrans, Quaternion.Euler(newRotation));
                    // if the players are still in radius then blast them away
                    if (Vector3.Distance(player1.transform.position, distanceCheck.transform.position) <= blastRadius)
                    {
                        if (player1RB)
                        {
                            player1RB.AddForce(gameObject.transform.right * blastForceScalar, ForceMode.Impulse);
                            //Debug.Log("Boom");

                        }
                    }

                    if (Vector3.Distance(player2.transform.position, distanceCheck.transform.position) <= blastRadius)
                    {
                        player2RB.AddForce(gameObject.transform.right * blastForceScalar, ForceMode.Impulse);
                    }
                    doOnce = true;
                }
                

                

                StartCoroutine(Rest());

                

            }


        }
    }

    void Strobe()
    {
        if (strobeDelayTimer <= 0f)
        {
            //Debug.Log("I should be flashing right now");
            strobeDelayTimer = strobeDelay;

            toggle = !toggle;

            if (toggle)
            {
                blastCircleSprite.GetComponent<SpriteRenderer>().enabled = true;
                meshRend.material.SetColor("_Color", Color.blue);
            }
            else
            {
                blastCircleSprite.GetComponent<SpriteRenderer>().enabled = false;

                meshRend.material.SetColor("_Color", originalColor);
            }
        }
        else
        {
            //Debug.Log("Loop loop");
            strobeDelayTimer -= Time.deltaTime;
        }
    }

    public void setNumShieldsBroken()
    {
        ++num_shields_broken;
    }

    void SetPlayers()
    {
        if (!player1)
        {
            if (GameObject.FindGameObjectWithTag("Player1").gameObject)
            {
                if (GameObject.FindGameObjectWithTag("Player1").gameObject.transform)
                {
                    if (GameObject.FindGameObjectWithTag("Player1").gameObject.transform.childCount > 0)
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").gameObject.transform.GetChild(0))
                        {
                            player1 = GameObject.FindGameObjectWithTag("Player1").gameObject.transform.GetChild(0).gameObject;
                            player1RB = player1.GetComponent<Rigidbody>();
                        }
                    }
                }
            }

        }
        if (!player2)
        {
            if (GameObject.FindGameObjectWithTag("Player2").gameObject)
            {
                if (GameObject.FindGameObjectWithTag("Player2").gameObject.transform)
                {
                    if (GameObject.FindGameObjectWithTag("Player1").gameObject.transform.childCount > 0)
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").gameObject.transform.GetChild(0))
                        {
                            player2 = GameObject.FindGameObjectWithTag("Player2").gameObject.transform.GetChild(0).gameObject;
                            player2RB = player2.GetComponent<Rigidbody>();

                        }
                    }
                }
            }

        }
    }

    IEnumerator Blast()
    {
        float waitTime;
        waitTime = Random.Range(2f, 4.5f);

        yield return  new WaitForSeconds(waitTime);

        isChargingBlast = true;
    }

    IEnumerator Rest()
    {
        float waitTime;
        waitTime = 2f;

        yield return new WaitForSeconds(waitTime);

        isChargingBlast = false;
        detonateTimer = 2.5f;
        doOnce = false;

    }
}
