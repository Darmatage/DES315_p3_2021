using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJN_Arm_Script : MonoBehaviour
{
    public bool IsLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(IsLeft)
        {
            transform.parent.parent.GetComponent<LJN_Weapon_Script>().LeftCanMove = false;
            
        }
        else
        {
            transform.parent.parent.GetComponent<LJN_Weapon_Script>().RightCanMove = false;
        }

        if (!transform.parent.parent.GetComponent<LJN_Weapon_Script>().LeftCanMove && !transform.parent.parent.GetComponent<LJN_Weapon_Script>().RightCanMove)
        {
            if (other.gameObject.GetComponent<BotBasic_Move>() != null && other.gameObject.name != transform.parent.parent.name)
            {
                var othermove = other.gameObject.GetComponent<BotBasic_Move>();
                othermove.isGrabbed = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
   
        if (IsLeft)
        {
            transform.parent.parent.GetComponent<LJN_Weapon_Script>().LeftCanMove = true;
            
        }
        else
        {
            transform.parent.parent.GetComponent<LJN_Weapon_Script>().RightCanMove = true;
        }

        if (collision.gameObject.GetComponent<BotBasic_Move>() != null)
        {

            var othermove = collision.gameObject.GetComponent<BotBasic_Move>();
            othermove.isGrabbed = false;
        }
    }
}
