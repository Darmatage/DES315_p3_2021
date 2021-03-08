using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QB_ControlInfo : MonoBehaviour
{

    public QB_OverheadAttack overhead;
    public QB_FlipAttack flip;

    


    // Start is called before the first frame update
    void Start()
    {
        overhead.attackKey = transform.parent.gameObject.GetComponent<playerParent>().action1Input;
        flip.attackKey = transform.parent.gameObject.GetComponent<playerParent>().action2Input;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
