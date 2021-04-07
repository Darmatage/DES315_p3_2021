using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_SpringiIntoPit : MonoBehaviour
{
    public float speed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == "PLAYER1_SLOT" || other.transform.root.name == "PLAYER2_SLOT")
        {
            Vector3 landingPos = new Vector3(0, 0, 0);
            Vector3 launchVec = GetLaunchVelocity(other.transform.localPosition, landingPos, speed);

            if(launchVec == Vector3.zero)
            {
                Debug.LogError("QB_SprintIntoPit: OnTriggerEnter: Launch impossible with current speed!");
                return;
            }

            Rigidbody rbody = other.gameObject.GetComponent<Rigidbody>();

            if(rbody)
            {
                rbody.velocity = launchVec;
                //rbody.angularVelocity = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            }
            else
            {
                Debug.LogError("QB_SpringiIntoPit: OnTriggerEnter: Could not get Rigidbody from colliding GameObject!");
                return;
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
