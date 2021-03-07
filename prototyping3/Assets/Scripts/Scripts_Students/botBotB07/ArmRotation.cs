using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public GameObject magnet;
    static float movementTimer = 0.0f;
    float Speed= 2f;
    bool RaiseArm = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && RaiseArm == false)
        {
            RaiseArm = true;
        }

        if(RaiseArm)
        {
            Vector3 Rot = transform.localRotation.eulerAngles;
            if (Rot.x > 270.5f || Rot.x < 1)
            {
                movementTimer += Time.deltaTime;
                Rot.x = -Mathf.SmoothStep(0, 112, Mathf.Clamp(movementTimer / Speed, 0, 1));
                transform.localRotation = Quaternion.Euler(Rot);
            }
            else
            {
                if(magnet.GetComponent<Magnet>().otherboi)
                {
                    magnet.GetComponent<Magnet>().otherboi.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    magnet.GetComponent<Magnet>().otherboi.gameObject.GetComponent<BotBasic_Move>().isGrabbed = false;
                    magnet.GetComponent<Magnet>().otherboi.transform.parent = magnet.GetComponent<Magnet>().OtherObjectRoot.transform;
                }
                movementTimer = 0;
                transform.localRotation = Quaternion.identity;
                RaiseArm = false;
            }
        }
    }
}
