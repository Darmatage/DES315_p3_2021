using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCover_Button : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject coverAbove;
    [SerializeField] private GameObject coverBelow;

    private Vector3 buttonPosition;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material pressedMaterial;

    private bool buttonTriggered = false;
    private bool wallsReset = true;
    [SerializeField] private float resetTimer;
    private float originalCD;

    private Vector3 aboveNewPosition;
    private Vector3 belowNewPosition;
    // Start is called before the first frame update
    void Start()
    {
        buttonPosition = button.GetComponent<Transform>().position;

        aboveNewPosition = coverAbove.transform.position;
        belowNewPosition = coverBelow.transform.position;

        originalCD = resetTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonTriggered && wallsReset)
        {
            aboveNewPosition.y -= 2;
            belowNewPosition.y += 2;
            wallsReset = false;
            buttonPosition.y -= .2f;
            button.GetComponent<Transform>().position = buttonPosition;
        }
        else if (buttonTriggered && !wallsReset)
        {
            resetTimer -= Time.deltaTime;
        }
        if (resetTimer <= 0.0f)
        {
            aboveNewPosition.y += 2;
            belowNewPosition.y -= 2;
            resetTimer = originalCD;
            wallsReset = true;
            buttonTriggered = false;
            button.GetComponent<MeshRenderer>().material = originalMaterial;
            buttonPosition.y += .2f;
            button.GetComponent<Transform>().position = buttonPosition;
        }
        coverAbove.transform.position = Vector3.Lerp(coverAbove.transform.position, aboveNewPosition, Time.deltaTime * 5.0f);
        coverBelow.transform.position = Vector3.Lerp(coverBelow.transform.position, belowNewPosition, Time.deltaTime * 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.root.gameObject.tag == "Player1") || (other.transform.root.gameObject.tag == "Player2") && wallsReset)
        {
            buttonTriggered = true; 
            button.GetComponent<MeshRenderer>().material = pressedMaterial;
        }
    }
}
