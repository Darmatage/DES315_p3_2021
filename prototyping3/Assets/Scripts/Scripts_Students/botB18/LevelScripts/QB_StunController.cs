using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_StunController : MonoBehaviour
{
    public float timerMax;

    private float timer;
    private float timer2;
    private GameObject obj1;
    private GameObject obj2;


    // Start is called before the first frame update
    void Start()
    {
        obj1 = null;
        obj2 = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(obj1)
        {
            timer -= Time.deltaTime;

            if(timer <= 0.0f)
            {
                timer = 0.0f;

                BotBasic_Move move = obj1.GetComponent<BotBasic_Move>();

                if(move)
                {
                    move.isGrabbed = false;
                }

                obj1 = null;
            }
        }

        if (obj2)
        {
            timer2 -= Time.deltaTime;

            if (timer2 <= 0.0f)
            {
                timer2 = 0.0f;

                BotBasic_Move move = obj2.GetComponent<BotBasic_Move>();

                if (move)
                {
                    move.isGrabbed = false;
                }

                obj2 = null;
            }
        }
    }

    public void Stun(GameObject obj)
    {
        GameObject curr = obj;
        bool success = false;

        while (curr.name != curr.transform.root.name)
        {
            BotBasic_Move move = curr.GetComponent<BotBasic_Move>();
            if (move)
            {
                if (!obj1)
                {
                    obj1 = curr;
                    success = true;
                    move.isGrabbed = true;
                    timer = timerMax;
                    break;
                }
                else if (!obj2)
                {
                    obj2 = curr;
                    success = true;
                    move.isGrabbed = true;
                    timer2 = timerMax;
                    break;
                }
            }

            curr = curr.transform.parent.gameObject;
        }

        if(!success)
        {
            Debug.Log("QB_StunController::Stun: Couldn't find BotBasic_Move Script!");
        }
    }
}
