using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_SpringiIntoPit : MonoBehaviour
{
    public float speed;
    public ParticleSystem particles;

    private QB_StunController stun;

    void Start()
    {
        stun = gameObject.GetComponent<QB_StunController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == "PLAYER1_SLOT" || other.transform.root.name == "PLAYER2_SLOT")
        {
            GameObject obj = other.transform.root.GetChild(0).gameObject;
            
            //while (obj.name != other.gameObject.transform.root.gameObject.name)
            //{
            //    if (obj.GetComponent<Rigidbody>())
            //    {
            //        break;
            //    }
            //    obj = obj.transform.parent.gameObject;
            //}
            //
            //if (obj.name == other.gameObject.transform.root.gameObject.name)
            //{
            //    Debug.LogError("QB_SprintIntoPit.cs: Object was not a part of a bot!");
            //}

            Vector3 landingPos = new Vector3(0, -2, 0);
            Vector3 launchVec = GetLaunchVelocity(obj.transform.position, landingPos, speed);

            if(launchVec == Vector3.zero)
            {
                Debug.LogError("QB_SprintIntoPit: OnTriggerEnter: Launch impossible with current speed!");
                return;
            }

            Rigidbody rbody = obj.gameObject.GetComponent<Rigidbody>();

            if(rbody)
            {
                rbody.velocity = launchVec;
                rbody.angularVelocity = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
            }
            else
            {
                Debug.LogError("QB_SpringiIntoPit: OnTriggerEnter: Could not get Rigidbody from colliding GameObject!");
                return;
            }

            stun.Stun(obj.gameObject);
            particles.Play();
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
