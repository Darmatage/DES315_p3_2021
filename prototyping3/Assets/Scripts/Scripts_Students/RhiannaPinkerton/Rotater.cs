using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private bool isClockwise;

    [SerializeField] private LevelRotationManager Manager;
    
    [SerializeField] private float CooldownTimer = 5f;

    private float timer = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.CooldownIsActive)
        {
            timer += Time.deltaTime;
            if (timer >= CooldownTimer)
            {
                Manager.CooldownIsActive = false;
                timer = 0f;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Manager.CooldownIsActive)
            return;
        if ((other.transform.root.gameObject.tag == "Player1") || (other.transform.root.gameObject.tag == "Player2"))
        {
            Manager.CooldownIsActive = true;
            if (isClockwise)
                Manager.RotateClockwise();
            else
            {
                Manager.RotateCounterClockwise();
            }
        }
    }
}
