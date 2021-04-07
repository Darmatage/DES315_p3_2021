using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_DamageText : MonoBehaviour
{
    public float rightVel = 1.5f;
    public float upVel = 14.0f;
    public float gravity = 45.0f;

    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // move towards the camera's up and right
        transform.position += Camera.main.transform.right * Time.deltaTime * rightVel;
        transform.position += Camera.main.transform.up * Time.deltaTime * upVel;

        // apply gravity based on cameraUp
        upVel -= gravity * Time.deltaTime;
    }
}
