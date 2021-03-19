using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotA14_LaserColliderScript : MonoBehaviour
{
    private bool inCollision = false;

    private int col = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(col > 1)
        {
            gameObject.SetActive(false);
            col = 0; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ++col;
    }
}
