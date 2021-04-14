using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerParent : MonoBehaviour{	
	public bool isPlayer1 = false;

	public string rotateAxis;
	public string moveAxis;
	public string jumpInput;
	
	public string action1Input;
	public string action2Input;
	public string action3Input;
	public string action4Input;	
	
    void Awake(){
        if (isPlayer1==true){
			rotateAxis = "p1Horizontal";
			moveAxis = "p1Vertical";
			jumpInput = "p1Jump";
			action1Input = "p1Fire1";
			action2Input = "p1Fire2";
			action3Input = "p1Fire3";
			action4Input = "p1Fire4";
		}else {
			rotateAxis = "p2Horizontal";
			moveAxis = "p2Vertical";
			jumpInput = "p2Jump";
			action1Input = "p2Fire1";
			action2Input = "p2Fire2";
			action3Input = "p2Fire3";
			action4Input = "p2Fire4";
		}	
    }
}
