using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkshatMadan_GoalScript : MonoBehaviour
{
    public bool isGoal2 = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            // Goal scored
            if (isGoal2)
                Debug.Log("P1 scored");
            else
                Debug.Log("P2 scored");
        }
    }
}
