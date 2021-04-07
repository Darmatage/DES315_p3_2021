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
    
    public bool CooldownIsActive = false;

    private float AngleCount = 0;

    [SerializeField] private float IncreaseAmount = 0.5f;

    [SerializeField] private float JumpHeight = 15f;

    // TODO USe this fo rthe rotaters to not sense rotating until done
    public bool isRotating = false;
    
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
                    CooldownIsActive = true;

                    Clockwise = false;
                    
                    //if (OpposingSide)
                    //    OpposingSide.SetActive(false);
                    
                    //Level.transform.localRotation = Quaternion.Euler(0f, 0f, EndRotation);
                }

                //Player1.transform.position = new Vector3(-8f, 8f, 0f);
                //Player2.transform.position = new Vector3(8f, 8f, 0f);
                
                Level.transform.Rotate(Vector3.forward, -IncreaseAmount);

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
                    CooldownIsActive = true;

                    CounterClockwise = false;
                    //Level.transform.localRotation = Quaternion.Euler(0f, 0f, EndRotation);
                    
                    
                    //if (OpposingSide)
                    //    OpposingSide.SetActive(false);
                }
                else
                {
                    //Player1.transform.position = new Vector3(-8f, 8f, 0f);
                    //Player2.transform.position = new Vector3(8f, 8f, 0f);
                    Level.transform.Rotate(Vector3.forward, IncreaseAmount);
                    //Level.transform.Rotate(Vector3.forward,
                    //    Mathf.Lerp(StartRotation, EndRotation, Time.deltaTime * (timer / RotateTime) * 2f));// timer / RotateTime));
                }
            }
        }
    }

    public void RotateClockwise()
    {
        isRotating = true;
        Clockwise = true;
        CounterClockwise = false;
        timer = 0f;
        AngleCount = 0;
        StartRotation = Level.transform.localRotation.z;
        GetEndRotation();
        
        Rigidbody P1 = Player1.GetComponentInChildren<Rigidbody>();
        if (P1)
        {
            Debug.Log("Force added");
            P1.AddForce(P1.centerOfMass + new Vector3(0f, JumpHeight*10, 0f), ForceMode.Impulse);
        }

        Rigidbody P2 = Player2.GetComponentInChildren<Rigidbody>();
        if (P2)
            P2.AddForce(P2.centerOfMass + new Vector3(0f, JumpHeight*10, 0f), ForceMode.Impulse);

    }

    public void RotateCounterClockwise()
    {
        isRotating = true;
        Clockwise = false;
        CounterClockwise = true;
        timer = 0f;
        AngleCount = 0;
        StartRotation = Level.transform.localRotation.eulerAngles.z;
        GetEndRotation();

        Rigidbody P1 = Player1.GetComponentInChildren<Rigidbody>();
        if (P1)
        {
            Debug.Log("Force added");
            P1.AddForce(P1.centerOfMass + new Vector3(0f, JumpHeight*10, 0f), ForceMode.Impulse);
        }

        Rigidbody P2 = Player2.GetComponentInChildren<Rigidbody>();
        if (P2)
            P2.AddForce(P2.centerOfMass + new Vector3(0f, JumpHeight*10, 0f), ForceMode.Impulse);

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
