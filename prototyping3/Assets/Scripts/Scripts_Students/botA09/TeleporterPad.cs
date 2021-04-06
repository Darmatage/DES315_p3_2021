using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPad : MonoBehaviour
{
    [SerializeField] private TeleporterPad linkedPad;

    private bool _isBeingTeleportedTo;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (!_isBeingTeleportedTo)
        {
            linkedPad._isBeingTeleportedTo = true;
            _isBeingTeleportedTo = true;

            var parent = other.transform.root.GetChild(0);
            var offset = parent.transform.position - transform.position;
            parent.transform.position = linkedPad.transform.position + offset;
        }
        
        yield return new WaitForSeconds(2f);
        _isBeingTeleportedTo = false;
    }
}
