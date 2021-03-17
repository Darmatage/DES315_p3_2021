using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JWymerPlow : MonoBehaviour
{
    public GameObject plow;

    public enum State
	{
        IDLE,
        RISING,
        FALLING
	}

    public State state = State.IDLE;

    public float maxAngle = -70;

    public float upTime = 1.0f;
    public float downTime = 1.0f;
    private float timer = 0.0f;

    public string button1;

    // Start is called before the first frame update
    void Start()
    {
        button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = 0.0f;

        switch(state)
		{
            case State.IDLE:

                if (Input.GetButtonDown(button1))
				{
                    state = State.RISING;
                    timer = upTime;
				}

                ;

                break;

            case State.RISING:


                timer -= Time.deltaTime;
                if (timer <= 0.0f)
				{
                    state = State.FALLING;
                    timer += downTime;
				}

                angle = Mathf.Lerp(maxAngle, 0, timer / upTime);

                break;

            case State.FALLING:

                timer -= Time.deltaTime;
                if (timer <= 0.0f)
				{
                    state = State.IDLE;
                    timer = 0.0f;
				}

                angle = Mathf.Lerp(0, maxAngle, timer / downTime);

                break;
		}

        plow.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
}
