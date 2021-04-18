using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_Healthbar : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // face upright
        Vector3 euler = transform.eulerAngles;
        euler.x = 0.0f;
        euler.z = 0.0f;
        transform.eulerAngles = euler;
    }
}
