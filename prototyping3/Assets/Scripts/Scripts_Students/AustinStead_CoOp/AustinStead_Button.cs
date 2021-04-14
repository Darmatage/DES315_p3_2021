using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AustinStead_Button : MonoBehaviour
{
    public float CooldownDuration = 1.0f;

    private float cooldownClock;


    public delegate void ButtonAction();
    public event ButtonAction ButtonActivated;


    public Material OpenMat;
    public Material ClosedMat;
    public GameObject ToggleObj;

    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        cooldownClock = 0;
        renderer = ToggleObj.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownClock > 0)
        {
            cooldownClock = Mathf.Max(0, cooldownClock - Time.deltaTime);
            
            if (cooldownClock == 0)
                CooldownComplete();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(cooldownClock <= 0)
        {
            cooldownClock = CooldownDuration;
            renderer.material = ClosedMat;

            if (ButtonActivated != null)
                ButtonActivated();

        }
    }

    private void CooldownComplete()
    {
        renderer.material = OpenMat;
    }

    public bool IsPressed()
    {
        return cooldownClock > 0;
    }

}
