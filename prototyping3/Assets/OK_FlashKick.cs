using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK_FlashKick : MonoBehaviour
{
    public bool weaponOut = false;
    public GameObject parent;

    private void OnCollisionEnter(Collision collision)
    {
        if (weaponOut)
        {
            if (collision.gameObject != parent)
            {
                if (collision.gameObject.GetComponent<Rigidbody>() != null)
                {
                    collision.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(this.transform.forward * 100, collision.gameObject.transform.position);
                }
            }
        }
    }
}
