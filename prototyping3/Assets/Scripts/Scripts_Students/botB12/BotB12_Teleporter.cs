using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotB12_Teleporter : MonoBehaviour
{
    [SerializeField] private Transform portalTransform;
    [SerializeField] private Light portalLight;
    [SerializeField] private ParticleSystem portalParticle;
    [SerializeField] private GameObject teleporterB;
    [SerializeField] private float timer;
    [SerializeField] private SpriteRenderer portalSprite;
    [SerializeField] private Color readyColor;
    [SerializeField] private Color usedColor;

    private float originalCD;
    private Vector3 offset;
    private Vector3 temp;
    private bool activateFlag = false;
    private ParticleSystem.MainModule particleMain;
    private bool callCoroutine = false;
    
    // Start is called before the first frame update
    void Start()
    {
        offset = portalTransform.position;
        originalCD = timer;
        particleMain = portalParticle.main;
    }

    // Update is called once per frame
    void Update()
    {
        temp = offset;
        temp.y += Mathf.Sin(Time.fixedTime * Mathf.PI) * .25f;
        portalTransform.position = temp;
        portalTransform.Rotate(Vector3.up * (Time.deltaTime * 40.0f));
        portalLight.intensity += Mathf.Sin(Time.fixedTime * Mathf.PI);

        if (timer <= 0.0f)
        {
            portalSprite.color = readyColor;
            portalLight.color = readyColor;
            particleMain.startColor = readyColor;
        }
        
        if (activateFlag)
        {
            portalSprite.color = usedColor;
            portalLight.color = usedColor;
            particleMain.startColor = usedColor;
        }

        if (callCoroutine)
        {
            StartCoroutine(delay(2.0f));
            callCoroutine = false;
        }

        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Bot") && (timer <= 0.0f) && (activateFlag == false))
        {
            timer = originalCD;
            teleporterB.GetComponent<BotB12_Teleporter>().activateFlag = true;
            teleporterB.GetComponent<BotB12_Teleporter>().callCoroutine = true;
            teleporterB.GetComponent<BotB12_Teleporter>().timer = originalCD;
            Vector3 newPos = new Vector3();
            newPos = teleporterB.transform.position;
            newPos.y += 1.0f;
            other.transform.position = newPos;
            activateFlag = true;
            callCoroutine = true;
        }
    }

    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
        activateFlag = false;
    }
}
