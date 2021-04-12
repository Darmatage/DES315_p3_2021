using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MK_FireObjBehavior : MonoBehaviour
{
    public float distance;
    public float lifetime;
    
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(this, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0,(-1 * distance * (Time.deltaTime / lifetime)));
    }
}
