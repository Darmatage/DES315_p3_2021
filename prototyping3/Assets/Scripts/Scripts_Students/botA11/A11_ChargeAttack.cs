using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Amogh;
using UnityEngine;

namespace Amogh
{
    public class A11_ChargeAttack : MonoBehaviour, A11_IAttack
    {
        public GameObject boomShurikenPrefab;
        public GameObject bassChild;

        public float force = 0.5f;
        public float AttackCooldownTimer = 5f;

        private float timer = 10f;
        void Start()
        {

        }
        

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
        }
        
        IEnumerator SpawnBass(AudioSource shurikenClip)
        {

            // Total time is 8 seconds
            // Start only after ~4 seconds is done (charge up sound)
            yield return new WaitForSeconds(4 - 1.3f);
            for (int i = 0; i < 5; ++i)
            {
                GameObject shuriken = Instantiate(boomShurikenPrefab,
                    bassChild.transform.position + bassChild.transform.forward, Quaternion.identity);

                var rb = shuriken.GetComponent<Rigidbody>();
                
                rb.velocity = bassChild.transform.forward * force;

                yield return new WaitForSeconds(1f);
            }
            
            // Wait until sound clip is done
            while (shurikenClip.isPlaying)
            {
                yield return new WaitForFixedUpdate();
            }

            bassChild.SetActive(false);
        }
        
        public void ButtonDown()
        {
            if (timer < AttackCooldownTimer)
                return;
            
            // Start charging up sound clip
            bassChild.SetActive(true);
            var shurikenClip = bassChild.GetComponent<AudioSource>();
            shurikenClip.time = 1.3f;
            shurikenClip.Play();

            StartCoroutine(SpawnBass(shurikenClip));

            timer = 0;
        }
        
        public void ButtonHeld()
        {
            
        }

        public void ButtonUp()
        {
            
        }
    }
}