using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSmashAttack : MonoBehaviour
{
    public string button1;
    public string button2;
    public string button3;
    public string button4;

    bool isPointedDown = false;

    public GameObject shockwave;
    public GameObject frontShield;

    public Vector3 shockwaveEndPos;

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

        shockwaveEndPos = new Vector3(shockwave.transform.position.x + 10, shockwave.transform.position.y, shockwave.transform.position.z + 10);
    }

    // Update is called once per frame
    void Update()
    {
        var botController = GetComponent<BotBasic_Move>();
        var rb = GetComponent<Rigidbody>();
        if(Input.GetButtonDown(button2))
        {
            rb.AddForce(rb.centerOfMass - new Vector3(0, botController.boostSpeed * 50, 0), ForceMode.Impulse);
            if(isPointedDown == false)
            {
                transform.Rotate(90, 0, 0);
                isPointedDown = true;
            }
        }
        if(botController.isGrounded == true)
        {
            isPointedDown = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8 && collision.gameObject == frontShield)
        {
            Instantiate(shockwave, transform.position, Quaternion.identity);
            Vector3.Lerp(transform.position, shockwaveEndPos, 2.0f);
        }
    }
}
