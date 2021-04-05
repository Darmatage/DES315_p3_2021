using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB09_WallButton : MonoBehaviour
{
    public bool move;
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

}
