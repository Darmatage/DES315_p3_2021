using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JWymerClaw : MonoBehaviour
{
    public float speed = 2f;
    
    Vector3 startingPos;

    enum ClawState
    { 
        RETRACTED,
        EXTENDING,
        EXTENDED,
        RETRACTING,
    };

    ClawState state = ClawState.RETRACTED;

   

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
		{
            state = ClawState.EXTENDING;
		}
        else if (state != ClawState.RETRACTED)
		{
            state = ClawState.RETRACTING;
		}
        
        Vector3 position = transform.localPosition;

        switch (state)
		{
            case ClawState.EXTENDING:
                
                position.z += speed * Time.deltaTime;
                break;

            case ClawState.EXTENDED:

                break;

            case ClawState.RETRACTING:
                
                position.z -= speed * Time.deltaTime;

                // Check if current position is at or before start
                if (!IsInFrontOfStart())
				{
                    position = startingPos;
                    state = ClawState.RETRACTED;
				}

                break;
        }

        transform.localPosition = position;
    }

    private bool IsInFrontOfStart()
	{
        Vector3 diff = transform.localPosition - startingPos;

        return (diff.z > 0);
	}
}
