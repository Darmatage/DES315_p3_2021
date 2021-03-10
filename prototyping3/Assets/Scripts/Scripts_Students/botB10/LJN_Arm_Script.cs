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
    }
}
