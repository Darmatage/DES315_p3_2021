using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public GameObject lava;
    public GameObject timerObj;
    public Timer timer;
    public bool scaled;
    // Start is called before the first frame update
    void Start()
    {
        scaled = false;
        timer = timerObj.GetComponent<Timer>();
    }




    void Update()
    {

        if ((int)timer.timeRemaining % 10 == 0 && timer.timeRemaining > 0 )
        {
            if (scaled == false)
            {
                scaled = true;
                ChangeSize(true);
            }

        }
        else
        {
            ChangeSize(false);
            scaled = false;
        }
    }

    public void ChangeSize(bool bigger)
    {

        if (bigger && scaled)
        {
            lava.transform.localScale += new Vector3(0, 4, 0);
        }
        


    }


}
