using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_CameraFollow : CameraFollow
{
	[SerializeField]
	new public Transform playerObj;

	new public bool isPlayer1;
	new public bool alsoFollowRotation = false;

	[SerializeField]
	new public Vector3 offsetCamera;

	[SerializeField]
	public float angleCamera;

	[SerializeField]
	private Space offsetPositionSpace = Space.Self;

	[SerializeField]
	private bool lookAt = true;

	void Start()
	{
		if (isPlayer1 == true)
		{
			if (GameObject.FindGameObjectWithTag("Player1").transform.GetChild(0) != null)
			{
				playerObj = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(0);
			}
		}
		else
		{
			if (GameObject.FindGameObjectWithTag("Player2").transform.GetChild(0) != null)
			{
				playerObj = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(0);
			}
		}
	}

	private void LateUpdate()
	{
		if (alsoFollowRotation == true)
		{
			MoveAndRotateCamera();
		}
		else { MoveCamera(); }
	}

	//script for camera to only follow movement, if alsoFollowRotation = false:
	new public void MoveCamera()
	{
		Vector3 pos = Vector3.Lerp((Vector3)transform.position, (Vector3)playerObj.position + offsetCamera, Time.fixedDeltaTime * 5);
		transform.position = new Vector3(pos.x, pos.y, pos.z);
		//transform.LookAt(playerObj);
	}

	//script for camera to follow movement and rotation, if alsoFollowRotation = true:
	new public void MoveAndRotateCamera()
	{
		Vector3 offsetCameraSide = new Vector3(offsetCamera.x, offsetCamera.y, offsetCamera.z);
		//if (playerObj == null) { Debug.LogWarning("Missing playerObj ref !", this); return; }
		// compute position 
		if (offsetPositionSpace == Space.Self) { transform.position = playerObj.TransformPoint(offsetCameraSide); }
		else { transform.position = playerObj.position + offsetCameraSide; }
		// compute rotation 
		if (lookAt) 
		{
			transform.LookAt(playerObj);
			transform.RotateAroundLocal(transform.right, Mathf.Deg2Rad * angleCamera);
		}
		//else {transform.rotation = playerObj.rotation;}
		else
		{
			Transform fromRot = gameObject.transform;
			Transform toRot = playerObj.transform;
			float speed = 0.01f;
			transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, playerObj.transform.rotation, Time.time * speed);
		}
	}
}
