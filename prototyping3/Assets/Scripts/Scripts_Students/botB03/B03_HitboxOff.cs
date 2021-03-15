using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_HitboxOff : MonoBehaviour
{
    public string target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.name == target)
        {
            gameObject.SetActive(false);
        }
    }
}
