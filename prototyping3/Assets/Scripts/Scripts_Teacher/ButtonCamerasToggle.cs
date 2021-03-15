using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCamerasToggle : MonoBehaviour{

	public bool camIsRotating = false; 
	public bool isPlayer1 = false;
	private CameraFollow p1Cam;
	private CameraFollow p2Cam;
	
	
    void Start(){
		//ned to change this so it updates when the game begins
        //UpdateCamera();
    }

	public void SwitchCameras(){
		p1Cam = GameObject.FindWithTag("Player1").GetComponentInChildren<CameraFollow>();
		p2Cam = GameObject.FindWithTag("Player2").GetComponentInChildren<CameraFollow>();
		camIsRotating = !camIsRotating;
		UpdateCamera();
	}

    void UpdateCamera(){
        if (camIsRotating == true){
			if (isPlayer1 == true){p1Cam.alsoFollowRotation = true;}
			else if (isPlayer1 == false){p2Cam.alsoFollowRotation = true;}
		} else if (camIsRotating == false){
			if (isPlayer1 == true){p1Cam.alsoFollowRotation = false;}
			else if (isPlayer1 == false){p2Cam.alsoFollowRotation = false;}
		}
    }

}
