using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_LifeTimer : MonoBehaviour
{
    public float lifespan = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);
    }
}
