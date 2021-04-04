using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicPoolDamage_DevonPeterson : MonoBehaviour
{
    public float ticktimer = .1f;
    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        timer = ticktimer;
        ticktimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ticktimer <= 0.0f)
        {
            gameObject.GetComponent<MeshCollider>().enabled = true;
            ticktimer = timer;
        }
        else if (ticktimer > 0.0f) 
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            ticktimer -= Time.deltaTime;
        }

    }
}
