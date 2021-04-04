using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleRotate : MonoBehaviour
{
    public float rotateCD;

    private Quaternion newRotation;
    private Transform thisTransform;
    private float originalCD;
    // Start is called before the first frame update
    void Start()
    {
        newRotation = transform.rotation;
        thisTransform = GetComponent<Transform>();
        originalCD = rotateCD;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateCD <= 0)
        {
            newRotation *= Quaternion.AngleAxis(90, Vector3.up);
            rotateCD = originalCD;
        }
        thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, newRotation, Time.deltaTime * 5.0f);
        rotateCD -= Time.deltaTime;
    }
}
