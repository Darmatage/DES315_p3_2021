using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave_Pushback : MonoBehaviour
{
    public Transform source;
    public float pushBack = 100;

    // Start is called before the first frame update
    void Start()
    {
        source = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Vector3 dir = new Vector3(other.transform.position.x - source.position.x, 0, other.transform.position.z - source.position.z).normalized;
        dir += Vector3.up * 0.5f;

        Rigidbody rb = other.transform.root.GetChild(0).GetComponent<Rigidbody>();
        rb.AddForce(dir * pushBack, ForceMode.Impulse);

        transform.GetComponent<Collider>().enabled = false;

    }
}
