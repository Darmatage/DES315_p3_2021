using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base scode from Robet Seiver

public class BotB09_cameraOverride : MonoBehaviour
{
    [Range(-10, 10)] public float Shift;

    private void Start()
    {
        var cam = transform.root.GetComponentInChildren<CameraFollow>();
        if (cam != null)
        {
            cam.offsetCamera += Vector3.up * Shift;
            cam.alsoFollowRotation = true;
        }
    }
}
