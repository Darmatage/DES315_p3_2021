using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_HitboxOff : MonoBehaviour
{
    public string target;
    public bool hitFlag = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == target)
        {
            if (hitFlag)
                gameObject.SetActive(false);
            else
                hitFlag = true;
        }
    }
}
