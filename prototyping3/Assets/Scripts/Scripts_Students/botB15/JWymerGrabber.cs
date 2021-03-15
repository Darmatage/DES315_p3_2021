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
    private Transform prevParent;

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
            grabbedObject.transform.SetParent(prevParent, true);
            basics.isGrabbed = false;

            grabbedObject = null;
            basics = null;
            prevParent = null;
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
                prevParent = otherObj.transform.parent;
                grabbedObject.transform.SetParent(transform, true);

                basics.isGrabbed = true;
                state = State.GRABBING;
			}
        }
	}
}
