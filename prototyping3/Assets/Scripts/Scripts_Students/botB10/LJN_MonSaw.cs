using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJN_MonSaw : MonoBehaviour
{
    public GameObject sec1;
    public GameObject sec2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveTail(bool state)
    {

        if(state)
        {
            sec1.GetComponent<HazardDamage>().damage = 5;
            sec2.GetComponent<HazardDamage>().damage = 5;
        }
        else
        {
            sec1.GetComponent<HazardDamage>().damage = 0;
            sec2.GetComponent<HazardDamage>().damage = 0;
        }
    }
}
