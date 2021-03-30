using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private bool isClockwise;

    [SerializeField] private LevelRotationManager Manager;
    
    [SerializeField] private float CooldownTimer = 3f;

    private float timer = 0f;

    private bool CooldownIsActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CooldownIsActive)
        {
            timer += Time.deltaTime;
            if (timer >= CooldownTimer)
            {
                CooldownIsActive = false;
                timer = 0f;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.root.gameObject.tag == "Player1") || (other.transform.root.gameObject.tag == "Player2"))
        {
            CooldownIsActive = true;
            if (isClockwise)
                Manager.RotateClockwise();
            else
            {
                Manager.RotateCounterClockwise();
            }
        }
    }
}
