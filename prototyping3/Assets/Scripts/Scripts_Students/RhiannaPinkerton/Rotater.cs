using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private bool isClockwise;

    [SerializeField] private LevelRotationManager Manager;
    
    [SerializeField] private float CooldownTimer = 5f;

    public float timer = 0f;

    private bool CooldownActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Manager.CooldownIsActive)
        if (CooldownActive)
        {
            timer += Time.deltaTime;
            if (timer >= CooldownTimer)
            {
                CooldownActive = false;
                timer = 0f;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (CooldownActive || Manager.isRotating)
            return;
        if ((other.transform.root.gameObject.tag == "Player1") || (other.transform.root.gameObject.tag == "Player2"))
        {
            CooldownActive = true;
            if (isClockwise)
                Manager.RotateClockwise();
            else
            {
                Manager.RotateCounterClockwise();
            }
        }
    }
}
