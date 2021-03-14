using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotB12_CannonFire : MonoBehaviour
{
    public string leftFire;
    public string rightFire;

    public GameObject cannonBall;

    private float cd = 2.5f;
    private float cooldownLeft;
    private float cooldownRight;

    public GameObject[] LeftCannonsSpots;
    public GameObject[] RightCannonsSpots;
    public GameObject[] LeftCannons;
    public GameObject[] RightCannons;

    private Vector3 firePosition;

    private AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        leftFire = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
        rightFire = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
        cooldownLeft = cd;
        cooldownRight = cd;
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(leftFire) && cooldownLeft <= 0.0f)
        {
            foreach (var cannonSpot in LeftCannonsSpots)
            {
                firePosition = cannonSpot.transform.position;
                firePosition.x = firePosition.x - .4f;
                GameObject ball = Instantiate(cannonBall, firePosition, Quaternion.identity) as GameObject;
                ball.GetComponent<Rigidbody>().AddForce(transform.right * -90);
                audioData.Play();
            }
            foreach (var cannon in LeftCannons)
            {
                cannon.GetComponent<Animation>().Play();
                cannon.GetComponentInChildren<ParticleSystem>().Play();
            }
            cooldownLeft = cd;
        }
        
        if (Input.GetButtonDown(rightFire) && cooldownRight <= 0.0f)
        {
            foreach (var cannonSpot in RightCannonsSpots)
            {
                firePosition = cannonSpot.transform.position;
                firePosition.x = firePosition.x + .4f;
                GameObject ball = Instantiate(cannonBall, firePosition, Quaternion.identity) as GameObject;
                ball.GetComponent<Rigidbody>().AddForce(transform.right * 90);
                audioData.Play();
            }
            foreach (var cannon in RightCannons)
            {
                cannon.GetComponent<Animation>().Play();
                cannon.GetComponentInChildren<ParticleSystem>().Play();
            }
            cooldownRight = cd;
        }

        cooldownLeft -= Time.deltaTime;
        cooldownRight -= Time.deltaTime;
    }
}