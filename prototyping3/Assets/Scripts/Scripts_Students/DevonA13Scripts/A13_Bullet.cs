using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A13_Bullet : MonoBehaviour
{

    public float speed = 100.0f;
    public string playertag;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != playertag) 
        {
            Destroy(gameObject);
        }
        
    }

    IEnumerator destroyBullet() 
    {
        yield return new WaitForSeconds(.6f);
        Destroy(gameObject);
    }
}
