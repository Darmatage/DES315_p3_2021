using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    public string button1;
    public string button2;
    public string button3;
    public string button4;

    bool isFacingUp = false;

    float fuel = 2.0f;

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

        if(Input.GetButton(button2))
        {
            if (fuel != 0.0f)
            {
                rb.AddForce(rb.centerOfMass + new Vector3(0f, botController.jumpSpeed * 10, 0f), ForceMode.Force);
                fuel -= Time.deltaTime;
                Debug.Log(fuel);
                if (isFacingUp == false)
                {
                    transform.Rotate(-90, 0, 0);
                    isFacingUp = true;
                }
            }
        }
        if(Input.GetButtonUp(button2))
        {
            isFacingUp = false;
            transform.Rotate(90, 0, 0);
            if(fuel == 0.0f && fuel != 2.0f)
            {
                fuel += Time.deltaTime;
            }
        }
        
    }
}
