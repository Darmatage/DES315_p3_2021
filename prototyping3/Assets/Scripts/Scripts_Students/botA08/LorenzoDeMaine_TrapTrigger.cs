using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorenzoDeMaine_TrapTrigger : MonoBehaviour
{
    public GameObject Trap;

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

        if(other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
            Trap.GetComponent<LorenzoDeMaine_FlingTrap>().triggerOn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
            Trap.GetComponent<LorenzoDeMaine_FlingTrap>().triggerOff();
    }
}
