using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OK_Projectile : MonoBehaviour
{

    GameObject pog;
    // Start is called before the first frame update
    private void Start()
    {
        pog = this.gameObject;
    }


    IEnumerator destroyParticle()
    {
        yield return new WaitForSeconds(0.02f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
            collision.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 2000, collision.transform.position);
        StartCoroutine("destroyParticle");
    }


}
