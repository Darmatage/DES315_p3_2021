using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    float speed = 6.0f;
    float gravity = 20.0f;

    Vector3 moveDirection = Vector3.zero;

    ConstantForce force;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var botController = GetComponent<BotBasic_Move>();
        var rb = GetComponent<Rigidbody>();

        if(Input.GetKey(KeyCode.R))
        {
            //moveDirection.y = botController.jumpSpeed;
            rb.AddForce(rb.centerOfMass + new Vector3(0f, botController.jumpSpeed * 10, 0f), ForceMode.Force);
            //rb.AddForce(rb.centerOfMass + new Vector3(0f, force.relativeForce.y * 10, 0f));
        }

    }
}
