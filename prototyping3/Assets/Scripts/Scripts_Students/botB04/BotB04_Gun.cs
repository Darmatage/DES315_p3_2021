using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BotB04.Controller
{
    public class BotB04_Gun : MonoBehaviour
    {
        public GameObject bulletRef;
        public GameObject bulletSpawn;

        public float recoilTime = .1f;
        public float recoilStrength = 1f;

        public AudioClip fireSound;
        public AudioClip blankSound;

        private Vector3 startPos;

        private AudioSource audioSource;
       

        private void Start()
        {
            startPos = transform.localPosition;
            audioSource = GetComponent<AudioSource>();
        }

        public void Fire()
        {
            GameObject bullet = GameObject.Instantiate(bulletRef, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            HazardDamage hazardAttr = bullet.GetComponent<HazardDamage>();

            if (gameObject.transform.root.tag == "Player1") { hazardAttr.isPlayer1Weapon = true; }
            if (gameObject.transform.root.tag == "Player2") { hazardAttr.isPlayer2Weapon = true; }

            audioSource.clip = fireSound;
            audioSource.Play();

            StartCoroutine(GunRecoil(recoilTime, recoilStrength));
        }

        public void Blank()
        {
            audioSource.clip = blankSound;
            audioSource.Play();
            StartCoroutine(GunRecoil(recoilTime, recoilStrength * .25f));
        }

        IEnumerator GunRecoil(float time, float strength)
        {
            Debug.Log("In gun recoil");
            
            float recoilTimer = time;
            while (recoilTimer > 0)
            {
                recoilTimer -= Time.deltaTime;
                transform.Translate(0,0,-1f * strength * Time.deltaTime);
                yield return null;
            }

            recoilTimer = time;
            while (recoilTimer > 0)
            {
                recoilTimer -= Time.deltaTime;
                transform.Translate(0, 0, 1f * strength * Time.deltaTime);
                yield return null;
            }

            transform.localPosition = startPos;

            yield return null;
        }
    }
}