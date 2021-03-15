using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{

    public bool rotate;
    public float rotationSpeed;
    public GameObject[] saws;

    // Start is called before the first frame update
    void Start()
    {
        rotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotate)
        {
            foreach(GameObject saw in saws)
            {
                saw.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            }
        }
    }
}
