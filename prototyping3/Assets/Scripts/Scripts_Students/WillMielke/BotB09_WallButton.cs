using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB09_WallButton : MonoBehaviour
{
    public bool move;
    public bool oneUse;
    public Material offMat;
    public Material onMat;

    // Start is called before the first frame update
    void Start()
    {
        move = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if(oneUse)
        {
            if (!move)
                move = true;
        }
        else
            move = !move;
        if(move)
        {
            gameObject.GetComponent<Renderer>().material = onMat;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = offMat;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (oneUse)
        {
            if (!move)
                move = true;
        }
        else
            move = !move;

        if (move)
        {
            gameObject.GetComponent<Renderer>().material = onMat;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = offMat;
        }
    }

}
