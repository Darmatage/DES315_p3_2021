using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class LevelRotationManager : MonoBehaviour
{

    [SerializeField] private GameObject Level;

    private GameObject Player1;
    private GameObject Player2;


    private float StartRotation = 0f;

    private bool Clockwise = false;
    private bool CounterClockwise = false;


    [SerializeField] private float RotateTime = 2f;
    private float timer = 0f;
    
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
            if (timer >= RotateTime)
            {
                Clockwise = false;
                Level.transform.rotation = Quaternion.Euler(0f, 0f, StartRotation + 180f);
            }
            //Vector3 rotation = Level.transform.rotation.eulerAngles;
            //rotation = Vector3.Lerp(new Vector3(0f, 0f, StartRotation), new Vector3(0f, 0f, StartRotation + 90f),
            //                        timer / RotateTime);
            //Level.transform.Rotate(rotation);
            
            Player1.transform.position = new Vector3(-10f, 15f, 0f);
            Player2.transform.position = new Vector3(10f, 15f, 0f);
            Level.transform.Rotate(Vector3.forward,
                Mathf.Lerp(StartRotation, StartRotation + 180f, Time.deltaTime * (timer / RotateTime)));// timer / RotateTime));
                                    //Mathf.Pow(growthTimer / GrowthTime, Interpolant
        }
        else if (CounterClockwise)
        {
            timer += Time.deltaTime;
            if (timer >= RotateTime)
            {
                Clockwise = false;
                Level.transform.rotation = Quaternion.Euler(0f, 0f, StartRotation - 180f);
            }
            Player1.transform.position = new Vector3(-10f, 15f, 0f);
            Player2.transform.position = new Vector3(10f, 15f, 0f);
            Level.transform.Rotate(Vector3.forward,
                Mathf.Lerp(StartRotation, StartRotation - 180f, Time.deltaTime * (timer / RotateTime)));// timer / RotateTime));
        }
    }

    public void RotateClockwise()
    {
        Clockwise = true;
        CounterClockwise = false;
        timer = 0f;
        StartRotation = Level.transform.rotation.z;
    }

    public void RotateCounterClockwise()
    {
        Clockwise = true;
        CounterClockwise = false;
        timer = 0f;
        StartRotation = Level.transform.rotation.z;
    }
}
