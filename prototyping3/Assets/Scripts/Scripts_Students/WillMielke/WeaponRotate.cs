using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{

    public bool rotate;
    public bool startup, slowdown;
    public float maxrotationSpeed;
    public float rotateSpeedIncrease;
    public float rotateSpeedDecrease;
    public float curRotSpeed;
    public GameObject[] saws;

    // Start is called before the first frame update
    void Start()
    {
        rotate = false;
        startup = slowdown = false;
        curRotSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startup)
        {
            curRotSpeed += rotateSpeedIncrease * Time.deltaTime;
            if (curRotSpeed > maxrotationSpeed)
                curRotSpeed = maxrotationSpeed;
        }
        else if (slowdown)
        {
            curRotSpeed -= rotateSpeedDecrease * Time.deltaTime;
            if (curRotSpeed < 0)
            {
                curRotSpeed = 0;
                startup = false;
                slowdown = false;
                rotate = false;
            }
        }

        if(rotate)
        {
            foreach(GameObject saw in saws)
            {
                saw.transform.Rotate(Vector3.right * curRotSpeed * Time.deltaTime);
            }
        }
    }

    public void StartSaws()
    {
        startup = true;
        slowdown = false;
        rotate = true;
    }

    public void SlowDownSaws()
    {
        slowdown = true;
        startup = false;
    }

}
