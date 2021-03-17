using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzBlade_rotation : MonoBehaviour
{
    float speed = 100;

    void Update()
    {
        transform.Rotate(Vector3.up, speed* Time.deltaTime);
    }
}
