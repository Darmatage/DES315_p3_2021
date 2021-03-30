using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AustinStead_Button : MonoBehaviour
{
    public float CooldownDuration = 1.0f;

    private float cooldownClock;

    public delegate void ButtonAction();
    public event ButtonAction ButtonActivated;


    // Start is called before the first frame update
    void Start()
    {
        cooldownClock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownClock = Mathf.Max(0, cooldownClock - Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        //if(cooldownClock <= 0)
        {
            //cooldownClock = CooldownDuration;

            if (ButtonActivated != null)
                ButtonActivated();
        }

    }





}
