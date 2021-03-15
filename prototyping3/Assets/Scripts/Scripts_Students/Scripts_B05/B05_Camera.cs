using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B05_Camera : MonoBehaviour
{
    private CameraFollow cam;
    public Transform camera_pos;

    private int cur = 0;

    private Vector3 norm = new Vector3(0.0f, 15.0f, -5.0f);
    private Vector3 mag = new Vector3(0.0f, 25.0f, 0.0f);

    private Vector3[] pos = {
        new Vector3(0.0f, 15.0f, -7.0f), // RECOVERING
        new Vector3(0.0f, 2.0f, -7.0f),   // AIMING
        new Vector3(0.0f, 9.0f, -11.0f),   // ATTACKING
        new Vector3(0.0f, 25.0f, -0.1f),  // ATTRACTING
        new Vector3(0.0f, 25.0f, -0.1f),  // REPELING
        new Vector3(0.0f, 9.0f, -11.0f),  // JUMPING
        new Vector3(0.0f, 15.0f, -7.0f)   // NORMAL
    };

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.root.GetComponentInChildren<CameraFollow>();
        if (cam != null)
        {
            cur = 6;
            cam.offsetCamera = pos[cur];
            cam.alsoFollowRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPos(int p)
    {
        cur = p;

        if (cam == null) return;

        if (p == 2)
        {
            cam.alsoFollowRotation = false;
        }
        else
        {
            cam.alsoFollowRotation = true;
        }

        cam.offsetCamera = pos[cur];
    }
}
