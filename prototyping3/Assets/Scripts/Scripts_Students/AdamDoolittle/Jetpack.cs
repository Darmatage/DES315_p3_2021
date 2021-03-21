using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    public GameObject JetBooster1;
    public GameObject JetBooster2;

    public string button1;
    public string button2;
    public string button3;
    public string button4;

    bool isFacingUp = false;
    bool canFly = true;
    bool isParticlePlaying = false;

    public float fuel = 2.0f;
    public float rocketSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
    }

    // Update is called once per frame
    void Update()
    {
        var botController = GetComponent<BotBasic_Move>();
        var rb = GetComponent<Rigidbody>();

        if(Input.GetButton(button1))
        {
            if (canFly == true)
            {
                rb.AddForce(rb.centerOfMass + new Vector3(0f, botController.jumpSpeed * rocketSpeed, 0f), ForceMode.Force);
                fuel -= Time.deltaTime;
                //Debug.Log(fuel);
                if (isFacingUp == false)
                {
                    transform.Rotate(-90, 0, 0);
                    isFacingUp = true;
                }
                if (isParticlePlaying == false)
                {
                    JetBooster1.GetComponent<ParticleSystem>().Play();
                    JetBooster2.GetComponent<ParticleSystem>().Play();
                    isParticlePlaying = true;
                }
            }
            if(fuel <= 0.0f)
            {
                canFly = false;
                JetBooster1.GetComponent<ParticleSystem>().Stop();
                JetBooster2.GetComponent<ParticleSystem>().Stop();
                isParticlePlaying = false;
            }
        }
        if(Input.GetButtonUp(button1))
        {
            isFacingUp = false;
            transform.Rotate(90, 0, 0);
        }
        if(botController.isGrounded == true && fuel <= 2.0f)
        {
            //Debug.Log(fuel);
            fuel += Time.deltaTime;
            canFly = true;
        }
    }
}
