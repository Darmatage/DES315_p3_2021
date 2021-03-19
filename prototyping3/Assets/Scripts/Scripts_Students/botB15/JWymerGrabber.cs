using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JWymerGrabber : MonoBehaviour
{
    public enum State
    {
        INACTIVE,
        ACTIVE,
        GRABBING
    }

    public State state = State.INACTIVE;

    private GameObject grabbedObject = null;
    private BotBasic_Move basics;

    public Transform tracker;

    //public Color[] stateColors = new Color[3];
    

    // Start is called before the first frame update
    void Start()
    {
        //stateColors[0] = Color.red;
        //stateColors[0] = Color.blue;
        //stateColors[0] = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedObject)
		{
            grabbedObject.transform.position = tracker.position;
            grabbedObject.transform.rotation = tracker.rotation;
		}
    }

    public void Activate()
	{
        if (state == State.INACTIVE)
            state = State.ACTIVE;
	}

    public void Deactivate()
	{
        if (state == State.ACTIVE)
            state = State.INACTIVE;
	}

    public void Release()
    {
        if (state == State.GRABBING)
        {
            basics.isGrabbed = false;

            grabbedObject = null;
            basics = null;
        }

        state = State.INACTIVE;
    }

    private void OnTriggerEnter(Collider other)
	{
        if (state == State.ACTIVE)
        {
            GameObject otherObj = other.gameObject;
            basics = otherObj.GetComponent<BotBasic_Move>();

            // Object is defined and is a bot.
            if (basics)
			{
                grabbedObject = otherObj;

                basics.isGrabbed = true;
                state = State.GRABBING;

                tracker.position = grabbedObject.transform.position;
                tracker.rotation = grabbedObject.transform.rotation;
			}
        }
	}
}
