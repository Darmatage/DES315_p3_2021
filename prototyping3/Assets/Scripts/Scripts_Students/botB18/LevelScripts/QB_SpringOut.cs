using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_SpringOut : MonoBehaviour
{
    public float timerMax;

    public float throwSpeed;

    private float timer = 0.0f;
    private float timer2 = 0.0f;
    private GameObject obj1;
    private GameObject obj2;

    private QB_StunController stun;

    // Start is called before the first frame update
    void Start()
    {
        stun = gameObject.GetComponent<QB_StunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;

            if(timer <= 0.0f)
            {
                timer = 0.0f;
                // throw player out of area
                Launch(obj1);
                stun.Stun(obj1);
            }
        }

        if (timer2 > 0.0f)
        {
            timer2 -= Time.deltaTime;

            if (timer2 <= 0.0f)
            {
                timer2 = 0.0f;
                // throw player2 out of area
                Launch(obj2);
                stun.Stun(obj2);
            }
        }
    }

    private void Launch(GameObject obj)
    {
        //GameObject obj = robj;
        //
        //while(obj.name != robj.transform.root.gameObject.name)
        //{
        //    if(obj.GetComponent<Rigidbody>())
        //    {
        //        break;
        //    }
        //    obj = obj.transform.parent.gameObject;
        //}
        //
        //if(obj.name == robj.transform.root.gameObject.name)
        //{
        //    Debug.LogError("QB_SprintOut.cs: Object was not a part of a bot!");
        //}

        Vector3 landingPos = new Vector3(7, 2, 0);

        if(obj.transform.position.x >= 0)
        {
            landingPos.x *= -1;
        }

        Vector3 launchVec = GetLaunchVelocity(obj.transform.position, landingPos, throwSpeed); 

        if(launchVec == Vector3.zero)
        {
            Debug.LogError("QB_SprintOut.cs: Launch: No Launch Vector was generated!");
            return;
        }

        Rigidbody rbody = obj.GetComponent<Rigidbody>();

        if(rbody)
        {
            rbody.velocity = launchVec;
            rbody.angularVelocity = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
        }
        else
        {
            Debug.LogError("QB_SpringOut.cs: Launch: Object didn't have a Rigidbody!");
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == "PLAYER1_SLOT" || other.transform.root.name == "PLAYER2_SLOT")
        {
            if (obj1 == other.transform.root.GetChild(0).gameObject || obj2 == other.transform.root.GetChild(0).gameObject)
            {
                return;
            }

            if (!obj1)
            {
                obj1 = other.transform.root.GetChild(0).gameObject;
                timer = timerMax;
            }
            else if (!obj2)
            {
                obj2 = other.transform.root.GetChild(0).gameObject;
                timer2 = timerMax;
            }
            else
            {
                Debug.LogError("QB_SprintOut.cs: OnCollisionEnter: Neither object is tested as free!");
            }

            Debug.Log(other.gameObject.name + " has entered the fire pit!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.name == "PLAYER1_SLOT" || other.transform.root.name == "PLAYER2_SLOT")
        {
            if (obj1 == other.transform.root.GetChild(0).gameObject)
            {
                obj1 = null;
                timer = 0.0f;
            }
            else if (obj2 == other.transform.root.GetChild(0).gameObject)
            {
                obj2 = null;
                timer2 = timerMax;
            }

            Debug.Log(other.gameObject.name + " is leaving the fire pit!");
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.transform.root.name == "PLAYER1_SLOT" || collision.transform.root.name == "PLAYER2_SLOT")
    //    {
    //        if(obj1 == collision.transform.root.GetChild(0).gameObject || obj2 == collision.transform.root.GetChild(0).gameObject)
    //        {
    //            return;
    //        }
    //
    //        if(!obj1)
    //        {
    //            obj1 = collision.transform.root.GetChild(0).gameObject;
    //            timer = timerMax;
    //        }
    //        else if(!obj2)
    //        {
    //            obj2 = collision.transform.root.GetChild(0).gameObject;
    //            timer2 = timerMax;
    //        }
    //        else
    //        {
    //            Debug.LogError("QB_SprintOut.cs: OnCollisionEnter: Neither object is tested as free!");
    //        }
    //
    //        Debug.Log(collision.gameObject.name + " has entered the fire pit!");
    //    }
    //}
    //
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.transform.root.name == "PLAYER1_SLOT" || collision.transform.root.name == "PLAYER2_SLOT")
    //    {
    //        if (obj1 == collision.transform.root.GetChild(0).gameObject)
    //        {
    //            obj1 = null;
    //            timer = 0.0f;
    //        }
    //        else if (obj2 == collision.transform.root.GetChild(0).gameObject)
    //        {
    //            obj2 = null;
    //            timer2 = timerMax;
    //        }
    //
    //        Debug.Log(collision.gameObject.name + " is leaving the fire pit!");
    //    }
    //}


    private Vector3 GetLaunchVelocity(Vector3 startPos, Vector3 targetPos, float speed, bool highArc = true)
    {
        Vector3 launchVelocity = Vector3.zero;
        Vector3 deltaPosition = targetPos - startPos;
        Vector3 horizontalDeltaPos = new Vector3(deltaPosition.x, 0, deltaPosition.z);
        Vector3 rotatedDeltaPosition = new Vector3(horizontalDeltaPos.magnitude, deltaPosition.y, 0);
        float angle1 = (180.0f / Mathf.PI) * Mathf.Atan((speed * speed + Mathf.Sqrt(Mathf.Pow(speed, 4) - Physics.gravity.magnitude * (Physics.gravity.magnitude * rotatedDeltaPosition.x * rotatedDeltaPosition.x + 2 * rotatedDeltaPosition.y * speed * speed))) / (Physics.gravity.magnitude * rotatedDeltaPosition.x));
        float angle2 = (180.0f / Mathf.PI) * Mathf.Atan((speed * speed - Mathf.Sqrt(Mathf.Pow(speed, 4) - Physics.gravity.magnitude * (Physics.gravity.magnitude * rotatedDeltaPosition.x * rotatedDeltaPosition.x + 2 * rotatedDeltaPosition.y * speed * speed))) / (Physics.gravity.magnitude * rotatedDeltaPosition.x));
        float minAngle = Mathf.Min(angle1, angle2);
        float maxAngle = Mathf.Max(angle1, angle2);
        float angle = highArc ? maxAngle : minAngle;
        if (!float.IsNaN(angle))
        {
            launchVelocity = Quaternion.AngleAxis(angle, Vector3.Cross(horizontalDeltaPos, Vector3.up)) * horizontalDeltaPos.normalized * speed;
        }
        return launchVelocity;
    }
}
