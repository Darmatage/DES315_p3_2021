using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_Bullet : MonoBehaviour
{
    public B03_Attack parent;
    public string target;
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.MovePosition(transform.position + 10.0f * transform.forward * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.name == target)
        {
            parent.ActivateMagnetize();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == target)
        {
            parent.ActivateMagnetize();
        }

        Destroy(gameObject);
    }
}
