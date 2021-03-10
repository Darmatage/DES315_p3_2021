using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB12_CannonFire : MonoBehaviour
{
    public string leftFire;
    public string rightFire;

    public GameObject cannonBall;

    private float cd = 4.0f;
    private float cooldownLeft;
    private float cooldownRight;

    public GameObject[] LeftCannons;
    public GameObject[] RightCannons;

    private Vector3 firePosition;

    // Start is called before the first frame update
    void Start()
    {
        leftFire = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        rightFire = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        cooldownLeft = cd;
        cooldownRight = cd;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(leftFire) && cooldownLeft <= 0.0f)
        {
            foreach (var cannon in LeftCannons)
            {
                firePosition = cannon.transform.position;
                firePosition.x = firePosition.x - .4f;
                GameObject ball = Instantiate(cannonBall, firePosition, Quaternion.identity) as GameObject;
                ball.GetComponent<Rigidbody>().AddForce(transform.right * -90);
            }
            cooldownLeft = cd;
        }
        
        if (Input.GetButtonDown(rightFire) && cooldownRight <= 0.0f)
        {
            foreach (var cannon in RightCannons)
            {
                firePosition = cannon.transform.position;
                firePosition.x = firePosition.x + .4f;
                GameObject ball = Instantiate(cannonBall, firePosition, Quaternion.identity) as GameObject;
                ball.GetComponent<Rigidbody>().AddForce(transform.right * 90);
            }
            cooldownRight = cd;
        }

        cooldownLeft -= Time.deltaTime;
        cooldownRight -= Time.deltaTime;
    }
}