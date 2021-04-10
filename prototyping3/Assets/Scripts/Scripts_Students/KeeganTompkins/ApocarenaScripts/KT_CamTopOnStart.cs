using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KT_CamTopOnStart : MonoBehaviour
{
    bool once = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (once)
        {
            GetComponent<ButtonCameraTopToggle>().SwitchCameras();
            once = false;
        }
        
    }
}
