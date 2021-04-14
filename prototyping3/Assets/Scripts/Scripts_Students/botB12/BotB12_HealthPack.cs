using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BotB12_HealthPack : MonoBehaviour
{
    [SerializeField] private Transform healthpackHorizTrans;
    [SerializeField] private Transform healthpackVertTrans;
    [SerializeField] private MeshRenderer healthpackHorizMesh;
    [SerializeField] private MeshRenderer healthpackVertMesh;
    [SerializeField] private float timer;
    [SerializeField] private Light pLight;
    [SerializeField] private ParticleSystem pParticle;

    private float originalCD;
    private static GameHandler handler;
    private bool resetPack;
    
    // Start is called before the first frame update
    void Start()
    {
        if (handler == null)
        {
            handler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }
        originalCD = timer;
    }

    // Update is called once per frame
    void Update()
    {
        healthpackHorizTrans.Rotate(Vector3.up * (Time.deltaTime * 40.0f));
        healthpackVertTrans.Rotate(Vector3.up * (Time.deltaTime * 40.0f));
        
        if (timer <= 0.0f && resetPack)
        {
            pLight.intensity = 5.0f;
            healthpackHorizMesh.enabled = true;
            healthpackVertMesh.enabled = true;
            resetPack = false;
            pParticle.Play();
        }
        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Bot") && (timer <= 0.0f))
        {
            timer = originalCD;
            handler.TakeDamage(other.gameObject.transform.root.tag, -2);
            pLight.intensity = 0;
            healthpackHorizMesh.enabled = false;
            healthpackVertMesh.enabled = false;
            resetPack = true;
            pParticle.Stop();
        }
    }
}
