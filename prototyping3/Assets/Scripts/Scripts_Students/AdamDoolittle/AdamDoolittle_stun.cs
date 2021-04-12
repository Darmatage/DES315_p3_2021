using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamDoolittle_stun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.tag == "Player1" || collision.gameObject.transform.root.tag == "Player2")
        {
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
