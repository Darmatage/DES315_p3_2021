using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_Healthbar : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // face towards camera direction
        transform.rotation = Camera.main.transform.rotation;
    }
}
