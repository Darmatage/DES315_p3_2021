using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KT_Rotate : MonoBehaviour
{
    public float RotSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rot = transform.rotation;
        transform.rotation = Quaternion.Euler(rot.eulerAngles + new Vector3(0.0f, 0.0f, RotSpeed * Time.deltaTime));
    }
}
