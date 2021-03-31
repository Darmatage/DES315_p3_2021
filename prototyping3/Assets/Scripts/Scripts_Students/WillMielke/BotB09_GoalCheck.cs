using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB09_GoalCheck : MonoBehaviour
{
    public bool p1entered, p2entered;
    // Start is called before the first frame update
    void Start()
    {
        p1entered = false;
        p2entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(p1entered && p2entered)
        {
            // won
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!p1entered && collision.transform.parent.gameObject.CompareTag("Player1"))
        {
            p1entered = true;
        }
        else if (!p2entered && collision.transform.parent.gameObject.CompareTag("Player2"))
        {
            p2entered = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (p1entered && collision.transform.parent.gameObject.CompareTag("Player1"))
        {
            p1entered = false;
        }
        else if (p2entered && collision.transform.parent.gameObject.CompareTag("Player2"))
        {
            p2entered = false;
        }
    }

}
