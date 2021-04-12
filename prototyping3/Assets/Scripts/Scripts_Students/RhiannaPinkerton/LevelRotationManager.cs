using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Experimental;
using UnityEngine;

public class LevelRotationManager : MonoBehaviour
{

    [SerializeField] private GameObject Level;

    private GameObject Player1;
    private GameObject Player2;


    private float StartRotation = 0f;
    private float EndRotation = 0f;

    private bool Clockwise = false;
    private bool CounterClockwise = false;


    //[SerializeField] private float RotateTime = 2f;
    private float timer = 0f;
    private float WaitTime = 1f;
    
    private float AngleCount = 0;

    [SerializeField] private float IncreaseAmount = 0.5f;

    [SerializeField] private float JumpHeight = 15f;

    // USe this fo rthe rotaters to not sense rotating until done
    public bool isRotating = false;


    [SerializeField] private Rotater ClockwiseRotater;
    [SerializeField] private Rotater CounterClockwiseRotater;
    
    // Start is called before the first frame update
    void Start()
    {
        Player1 = GameObject.FindWithTag("Player1");
        Player2 = GameObject.FindWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Clockwise)
        {
            timer += Time.deltaTime;
            if (timer >= WaitTime)
            {

                AngleCount += IncreaseAmount;
                //timer += Time.deltaTime;
                //if (timer >= RotateTime)
                if (AngleCount >= 90f)
                {
                    isRotating = false;
                    
                    Clockwise = false;
                    
                    ClockwiseRotater.StartCooldown();
                    CounterClockwiseRotater.StartCooldown();
                }
                else
                {
                    Level.transform.Rotate(Vector3.forward, -IncreaseAmount);

                }

                //Level.transform.Rotate(Vector3.forward,
                //    Mathf.Lerp(StartRotation, EndRotation, Time.deltaTime * (timer / RotateTime)));// timer / RotateTime));
            }
        }
        else if (CounterClockwise)
        {
            timer += Time.deltaTime;
            if (timer >= WaitTime)
            {
                AngleCount += IncreaseAmount;
                //timer += Time.deltaTime;
                //if (timer >= RotateTime)
                if (AngleCount >= 90f)
                {
                    isRotating = false;

                    CounterClockwise = false;
                    
                    ClockwiseRotater.StartCooldown();
                    CounterClockwiseRotater.StartCooldown();
                }
                else
                {
                    Level.transform.Rotate(Vector3.forward, IncreaseAmount);
                    //Level.transform.Rotate(Vector3.forward,
                    //    Mathf.Lerp(StartRotation, EndRotation, Time.deltaTime * (timer / RotateTime) * 2f));// timer / RotateTime));
                }
            }
        }
    }

    public void RotateClockwise()
    {
        Clockwise = true;
        CounterClockwise = false;
        StartRotation = Level.transform.localRotation.eulerAngles.z;
        GetEndRotation();
        
        InitRotation();
    }

    public void RotateCounterClockwise()
    {
        Clockwise = false;
        CounterClockwise = true;
        StartRotation = Level.transform.localRotation.eulerAngles.z;
        GetEndRotation();

        InitRotation();
    }

    private void InitRotation()
    {
        isRotating = true;
        timer = 0f;
        AngleCount = 0;
        
        Rigidbody P1 = Player1.GetComponentInChildren<Rigidbody>();
        if (P1)
        {
            P1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            P1.AddForce(P1.centerOfMass + new Vector3(0f, JumpHeight*10, 0f), ForceMode.Impulse);
        }

        Rigidbody P2 = Player2.GetComponentInChildren<Rigidbody>();
        if (P2)
        {
            P2.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            P2.AddForce(P2.centerOfMass + new Vector3(0f, JumpHeight * 10, 0f), ForceMode.Impulse);
        }
    }


    private void GetEndRotation()
    {
        if (Clockwise)
        {
            if (StartRotation >= 270f)
            {
                EndRotation = 180f;
            }
            else if (StartRotation >= 180f)
            {
                EndRotation = 90f;
            }
            else if (StartRotation >= 90f)
            {
                EndRotation = 0f;
            }
            else if (StartRotation >= 0f)
            {
                EndRotation = 270f;
            }
        }
        else if (CounterClockwise)
        {
            if (StartRotation >= 270f)
            {
                EndRotation = 0f;
            }
            else if (StartRotation >= 180f)
            {
                EndRotation = 270f;
            }
            else if (StartRotation >= 90f)
            {
                EndRotation = 180f;
            }
            else if (StartRotation >= 0f)
            {
                EndRotation = 90f;
            }
        }
    }
}
