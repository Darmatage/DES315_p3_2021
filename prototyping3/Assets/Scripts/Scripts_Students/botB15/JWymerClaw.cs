using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JWymerClaw : MonoBehaviour
{
    public float speed = 2f;

    public GameObject claw;
    public GameObject cable;
    
    Vector3 startingClawPos;
    Vector3 startingCablePos;
    Vector3 startingCableScale;

    float extendDist = 0.0f;

    public float cooldownTime = 1.0f;
    public float extendTime = 1.0f;
    private float timer = 0.0f;

    enum ClawState
    { 
        RETRACTED,
        EXTENDING,
        EXTENDED,
        RETRACTING,
        COOLDOWN
    };

    ClawState state = ClawState.RETRACTED;

    public JWymerGrabber grabber;

    public string button2, button3, button4;

    public AudioSource audio;
    public AudioClip extendSFX;
    public AudioClip retractSFX;
    public AudioClip cooloffSFX;

    // Start is called before the first frame update
    void Start()
    {
        startingClawPos = claw.transform.localPosition;
        startingCablePos = cable.transform.localPosition;
        startingCableScale = cable.transform.localScale;
        button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
        button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
		{
            case ClawState.RETRACTED:

                if (Input.GetButtonDown(button2) && (grabber.state != JWymerGrabber.State.GRABBING))
				{
                    state = ClawState.EXTENDING;
                    timer = extendTime;
                    grabber.Activate();

                    audio.clip = extendSFX;
                    audio.Play();
                }

                break;



            case ClawState.EXTENDING:
                
                extendDist += speed * Time.deltaTime;

                timer -= Time.deltaTime;

                if (timer < 0.0f || grabber.state == JWymerGrabber.State.GRABBING)
				{
                    state = ClawState.RETRACTING;

                    audio.clip = retractSFX;
                    audio.Play();

                    grabber.Deactivate();
				}

                break;



            case ClawState.RETRACTING:
                
                extendDist -= speed * Time.deltaTime;

                // Check if current position is at or before start
                if (!IsInFrontOfStart())
				{
                    extendDist = 0.0f;

                    grabber.Release();
                    state = ClawState.COOLDOWN;
                    timer = cooldownTime;

                    audio.Stop();
				}

                break;



            case ClawState.COOLDOWN:

                timer -= Time.deltaTime;

                if (timer <= 0.0f)
				{
                    state = ClawState.RETRACTED;

                    audio.clip = cooloffSFX;
                    audio.Play();
				}

                break;
        }

        claw.transform.localPosition = startingClawPos + new Vector3(0, 0, extendDist);
        cable.transform.localPosition = startingCablePos + new Vector3(0, 0, extendDist / 2);
        cable.transform.localScale = startingCableScale + new Vector3(0, 0, extendDist);
    }

    private bool IsInFrontOfStart()
    {
        return extendDist > 0;
    }
}
