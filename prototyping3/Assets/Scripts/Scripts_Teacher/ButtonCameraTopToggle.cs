using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCameraTopToggle : MonoBehaviour{

	public bool camTopBig = false; 
	public GameObject camRenderSmall;
	public GameObject camRenderBig;

	void Start(){
		camRenderSmall.SetActive(false);
		camRenderBig.SetActive(false);
	}


	public void SwitchCameras(){
		camTopBig = !camTopBig;
		UpdateCamera();
	}

    void UpdateCamera(){
        if (camTopBig == true){
			camRenderSmall.SetActive(false);
			camRenderBig.SetActive(true);
		} else if (camTopBig == false){
			camRenderSmall.SetActive(false);
			camRenderBig.SetActive(false);
		}
    }

}
