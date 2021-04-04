using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AustinStead_AirVent : MonoBehaviour
{
    public float Acceleration = 3f;
    public float MaxYSpeed = 10f;
    public float NegativeYSpeedMod = 2f;

    public bool Enabled = true;

    MeshCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<MeshCollider>();
        collider.enabled = Enabled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
        {
            SetGravity(other.transform.root.gameObject, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
        {
            SetGravity(other.transform.root.gameObject, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.tag == "Player1" || other.transform.root.tag == "Player2")
        {
            UpdateGravity(other.transform.root.gameObject);
        }
    }


    private void SetGravity(GameObject root, bool enter)
    {
        Rigidbody[] rbList = root.GetComponentsInChildren<Rigidbody>(false);
        //Rigidbody rb = GetComponentInChildren<Rigidbody>(false);
        if(enter)
        {
            foreach (Rigidbody rb in rbList)
            {
                rb.useGravity = false;
            }
        }
        else
        {
            foreach (Rigidbody rb in rbList)
            {
                rb.useGravity = true;
            }
        }
    }


    private void UpdateGravity(GameObject root)
    {
        Rigidbody[] rbList = root.GetComponentsInChildren<Rigidbody>(false);
        //Rigidbody rb = GetComponentInChildren<Rigidbody>(false);

        foreach (Rigidbody rb in rbList)
        {
            if(rb.velocity.y < 0)
                rb.velocity = rb.velocity + transform.up * Acceleration * NegativeYSpeedMod * Time.deltaTime;
            else
                rb.velocity = rb.velocity + transform.up * Acceleration * Time.deltaTime;

            if (rb.velocity.y > MaxYSpeed)
                rb.velocity = new Vector3(rb.velocity.x, MaxYSpeed, rb.velocity.z);

            rb.angularVelocity = new Vector3();
            rb.transform.rotation = new Quaternion(0, rb.transform.rotation.y, 0, rb.transform.rotation.w);
        }

    }


}
