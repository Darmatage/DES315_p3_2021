using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public GameObject Wall;
    public BotB09_WallButton button;
    public Vector3 movePosition;

    public Vector3 initialpos;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        t = 0.0f;
        initialpos = Wall.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(button.move)
        {
            
            t += Time.deltaTime;
            if (t > 1.0f)
            {
                t = 1.0f;
                Wall.transform.position = movePosition;
            }
                
        }
        else
        {
            t -= Time.deltaTime;
            if (t < 0.0f)
            {
                t = 0.0f;
                Wall.transform.position = initialpos;
            }
                
        }
        if(t > 0.0f && t < 1.0f)
            Wall.transform.position = initialpos * (1 - t) + movePosition * t;
    }

}
