using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_Camera : MonoBehaviour
{
    public Transform camera_pos;

    private Vector3 attack_pos = new Vector3(0.0f, 2.0f, 5.0f); // calc coordinates to see bot from behind
    private Vector3 normal_pos = new Vector3(0.0f, 15.0f, 5.0f);
    private Vector3 aim_pos = new Vector3(0.0f, 3.0f, 8.0f); // calc as well
    private Vector3 mag_pos = new Vector3(0.0f, 20.0f, 5.0f);

    // Start is called before the first frame update
    void Start()
    {
        CameraFollow cam = transform.root.GetComponentInChildren<CameraFollow>();
        if (cam != null)
        {
            //cam.transform.position = camera_pos.position;
            cam.offsetCamera = normal_pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
