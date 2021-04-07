using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_DRC : MonoBehaviour
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
        collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 5.3f, collision.gameObject.transform.position.z);
    }
}
