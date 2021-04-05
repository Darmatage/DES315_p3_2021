using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_SpringOut : MonoBehaviour
{
    public float timerMax;

    private float timer = 0.0f;
    private float timer2 = 0.0f;
    private GameObject obj1;
    private GameObject obj2;
    

    // Start is called before the first frame update
    void Start()
    {
        
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
            }
        }

        if (timer2 > 0.0f)
        {
            timer2 -= Time.deltaTime;

            if (timer2 <= 0.0f)
            {
                timer2 = 0.0f;
                // throw player2 out of area
            }
        }
    }

    private void Launch(GameObject obj)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.name == "PLAYER1_SLOT" || collision.transform.root.name == "PLAYER2_SLOT")
        {
            if(obj1)
            {
                obj1 = collision.gameObject;
                timer = timerMax;
            }
            else
            {
                obj2 = collision.gameObject;
                timer2 = timerMax;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.root.name == "PLAYER1_SLOT" || collision.transform.root.name == "PLAYER2_SLOT")
        {
            if (obj1 == collision.gameObject)
            {
                obj1 = null;
                timer = 0.0f;
            }
            else if(obj2 == collision.gameObject)
            {
                obj2 = null;
                timer2 = timerMax;
            }
        }
    }


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
