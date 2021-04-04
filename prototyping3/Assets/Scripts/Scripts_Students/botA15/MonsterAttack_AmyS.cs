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
    private MeshRenderer meshRend;
    private Color originalColor;

    private GameObject player1;
    private GameObject player2;
    private Rigidbody player1RB;
    private Rigidbody player2RB;

    public float blastRadius = 16f;

    bool isChargingBlast = false;

    public static float strobeDelay = .15f;
    float strobeDelayTimer = strobeDelay;
    bool toggle = false;
    float detonateTimer = 2.5f; // in seconds

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
            if (Vector3.Distance(player1.transform.position, distanceCheck.transform.position) <= blastRadius
                || Vector3.Distance(player2.transform.position, distanceCheck.transform.position) <= blastRadius)
            {
                if(!isChargingBlast)
                    StartCoroutine(Blast());
            }
            else
            {
                if(!isChargingBlast)
                    StopCoroutine(Blast());
            }

            if (isChargingBlast)
            {
                if (detonateTimer >= 0)
                {
                    //Debug.Log("I am in the first if statement");
                    Strobe();
                    detonateTimer -= Time.deltaTime;
                }
                else
                {
                    meshRend.material.SetColor("_Color", originalColor);

                    if(Vector3.Distance(player1.transform.position, distanceCheck.transform.position) <= blastRadius)
                    {
                        if(player1RB)
                        {
                            player1RB.AddForce(-player1.transform.forward * 250f, ForceMode.Impulse);
                            //Debug.Log("Boom");

                        }
                    }

                    if (Vector3.Distance(player2.transform.position, distanceCheck.transform.position) <= blastRadius)
                    {
                        player2RB.AddForce(-player2.transform.forward * 250f, ForceMode.Impulse);
                    }

                    isChargingBlast = false;
                    detonateTimer = 2.5f;
                }

                
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
                meshRend.material.SetColor("_Color", Color.blue);
            else
                meshRend.material.SetColor("_Color", originalColor);
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
}
