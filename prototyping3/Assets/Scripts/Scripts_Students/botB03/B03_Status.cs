using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_Status : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }
}
