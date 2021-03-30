using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTopDown : MonoBehaviour
{
	
	public Transform p1;
	public Transform p2;
	public Camera cam;
	//public float speed = 2.0f;
	
	//public float camHeight = 20;
	
    // Start is called before the first frame update
    void Start(){
		cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update(){
		FixedCameraFollowSmooth(cam, p1, p2);
		// Vector3 playersCenter = (p1.position - p2.position).normalized;
		// camHeight = playersCenter.y + 5f;
		
        // Vector3 destPos = new Vector3(playersCenter.x, camHeight, playersCenter.z);
		// transform.position = Vector3.Lerp(transform.position, destPos, speed * Time.deltaTime);		
    }

	public void loadPlayers(GameObject player1, GameObject player2){
		p1 = player1.transform;
		p2 = player2.transform;
	}

	 public void FixedCameraFollowSmooth(Camera cam, Transform p1, Transform p2){
     // How many units should we keep from the players
     float zoomFactor = 1.5f;
     float followTimeDelta = 0.8f;
 
     // Midpoint we're after
     Vector3 midpoint = (p1.position + p2.position) / 2f;
 
     // Distance between objects
     float distance = (p1.position - p2.position).magnitude;
 
     // Move camera a certain distance
     Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;
 
     // Adjust ortho size if we're using one of those
     if (cam.orthographic)
     {
         // The camera's forward vector is irrelevant, only this size will matter
         cam.orthographicSize = distance;
     }
     // You specified to use MoveTowards instead of Slerp
     cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);
         
     // Snap when close enough to prevent annoying slerp behavior
     if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
         cam.transform.position = cameraDestination;
 }


}
