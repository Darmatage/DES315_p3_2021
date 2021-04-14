using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapHalf : MonoBehaviour
{
    [SerializeField] private Transform buttonTrans;
    [SerializeField] private MeshRenderer buttonMesh;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material pressedMaterial;
    private Vector3 buttonPosition;

    [SerializeField] private Transform spikeField;
    private Vector3 raisedPosition;
    private bool activateSpikes = false;
    private bool reset = false;
    private bool callCoroutine = true;
    
    [SerializeField] private float resetTimer;
    private float originalCD;
    private bool buttonTriggered = false;
    private bool raiseButton = true;

    [SerializeField] private GameObject spikeTrapOtherHalf;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonPosition = buttonTrans.position;
        originalCD = resetTimer;

        raisedPosition = spikeField.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonTriggered)
        {
            buttonPosition.y -= .2f;
            buttonTrans.position = buttonPosition;
            buttonMesh.material = pressedMaterial;
            buttonTriggered = false;
        }
        
        if (resetTimer <= 0.0f && raiseButton == false)
        {
            raiseButton = true;
            buttonPosition.y += .2f;
            buttonTrans.position = buttonPosition;
            buttonMesh.material = originalMaterial;
        }
        
        resetTimer -= Time.deltaTime;

        if (activateSpikes)
        {
            spikeField.transform.position =
                Vector3.Lerp(spikeField.transform.position, raisedPosition, Time.deltaTime * 15.0f);
            if (callCoroutine)
            {
                StartCoroutine(delayDrop(1f));
                callCoroutine = false;
            }
        }
        else if (!activateSpikes && reset)
        {
            spikeField.transform.position =
                Vector3.Lerp(spikeField.transform.position, raisedPosition, Time.deltaTime * 15.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.name.Contains("Bot")) && (resetTimer <= 0.0f) && (buttonTriggered == false))
        {
            resetTimer = originalCD;
            spikeTrapOtherHalf.GetComponent<SpikeTrapHalf>().buttonTriggered = true;
            spikeTrapOtherHalf.GetComponent<SpikeTrapHalf>().resetTimer = originalCD;
            spikeTrapOtherHalf.GetComponent<SpikeTrapHalf>().raiseButton = false;
            buttonTriggered = true;
            raiseButton = false;
            reset = false;
            raisedPosition.y += 1.0f;
            StartCoroutine(delayRaise(1f));
        }
    }
    
    IEnumerator delayRaise(float time)
    {
        yield return new WaitForSeconds(time);
        activateSpikes = true;
    }
    
    IEnumerator delayDrop(float time)
    {
        yield return new WaitForSeconds(time);
        raisedPosition.y -= 1.0f;
        activateSpikes = false;
        reset = true;
        callCoroutine = true;
    }
}
