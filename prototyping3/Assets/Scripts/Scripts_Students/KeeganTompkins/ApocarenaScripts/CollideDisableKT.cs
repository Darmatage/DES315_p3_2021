using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDisableKT : MonoBehaviour
{
    bool collided = false;
    private void Update()
    {
        if (collided)   
            GetComponent<SphereCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        collided = true;
    }
}
