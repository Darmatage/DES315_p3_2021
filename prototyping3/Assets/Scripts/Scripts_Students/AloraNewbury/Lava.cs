using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public GameObject Lava;
    private Timer timer = GetComponent<Timer>();
    private float remaining;
    // Start is called before the first frame update
    void Start()
    {
        remaining = timer.timeRemaining; 
    }




    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);

        // If you don't want an eased scaling, replace the above line with the following line
        //   and change speed to suit:
        // transform.localScale = Vector3.MoveTowards (transform.localScale, targetScale, speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangeSize(true);
        if (Input.GetKeyDown(KeyCode.DownjArrow))
            ChangeSize(false);
    }

    public void ChangeSize(bool bigger)
    {

        if (bigger)
            currScale++;
        else
            currScale--;

        currScale = Mathf.Clamp(currScale, minSize, maxSize + 1);

        targetScale = baseScale * currScale;
    }


}
