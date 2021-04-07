using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalLaser : MonoBehaviour
{
    public GameObject HazardCollider;
    public GameObject Laser;

    public float ChargeTime = 1.0f;

    public AudioClip SFX_Charge;
    public AudioClip SFX_Boom;

    public float Time = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Charge());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Charge()
    {
        GetComponent<AudioSource>().PlayOneShot(SFX_Charge);
        yield return new WaitForSeconds(ChargeTime);
        StartCoroutine(Boom());
    }

    IEnumerator Boom()
    {
        GetComponent<AudioSource>().PlayOneShot(SFX_Boom);
        HazardCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
