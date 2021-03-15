using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A08_ProjectileParticle : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifetime = 0.5f;


    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
