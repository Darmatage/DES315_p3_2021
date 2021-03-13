using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A08_Projectile : MonoBehaviour
{
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

        if (other.transform.root.tag != this.transform.root.tag &&
            other.transform.root.tag != "Untagged")
        {
            //Destroy(this.gameObject);
        }
    }
}
