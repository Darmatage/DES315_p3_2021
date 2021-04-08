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
    
    // Update is called once per frame
    void Update()
    {
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

    public void StartCooldown()
    {
        CooldownActive = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (CooldownActive || Manager.isRotating)
            return;
        if ((other.transform.root.gameObject.tag == "Player1") || (other.transform.root.gameObject.tag == "Player2"))
        {
            if (isClockwise)
                Manager.RotateClockwise();
            else
            {
                Manager.RotateCounterClockwise();
            }
        }
    }
}
