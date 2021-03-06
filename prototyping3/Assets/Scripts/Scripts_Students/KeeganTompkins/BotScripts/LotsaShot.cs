﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotsaShot : MonoBehaviour
{
    public GameObject ShootFrom;

    public float BulletSpeed = 20.0f;

    public GameObject BulletPrefab = null;

    string button2;

    public int ShotsPerFlip = 10;

    int ShotsLeft = 0;

    public float Fliptime = 0.8f;

    public AudioClip ShootSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Flip()
    {
        ShotsLeft = ShotsPerFlip;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = ShootFrom.transform.position;
        bullet.GetComponent<Rigidbody>().velocity = (transform.up * BulletSpeed);
        if (gameObject.transform.root.tag == "Player1") { bullet.GetComponent<HazardDamage>().isPlayer1Weapon = true; }
        if (gameObject.transform.root.tag == "Player2") { bullet.GetComponent<HazardDamage>().isPlayer2Weapon = true; }

        GetComponent<AudioSource>().PlayOneShot(ShootSound);

        if (--ShotsLeft > 0)
        {
            yield return new WaitForSeconds(Fliptime / ShotsPerFlip);
            StartCoroutine(Shoot());
        }
    }
}
