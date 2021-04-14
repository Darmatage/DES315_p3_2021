using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AustinStead_Camera : MonoBehaviour
{
    private void OnEnable()
    {
        GameHandler.onBattleStart += DelayedCameras;
    }

    private void OnDisable()
    {
        GameHandler.onBattleStart -= DelayedCameras;
    }

    //Saw that Robert did this to override the camera that is set on battle start
    private void DelayedCameras()
    {
        Invoke(nameof(SetCameras), 0.1f);
    }
    private void SetCameras()
    {
        CameraFollow[] cameras = FindObjectsOfType<CameraFollow>();

        foreach (CameraFollow cam in cameras)
        {
            //cam.offsetCamera = new Vector3(0, 20, -15);
            //cam.offsetCamera = new Vector3(0, 3, -10);
            cam.offsetCamera = new Vector3(0, 6, -10);
            cam.alsoFollowRotation = true;
        }
    }
}
