using System.Collections;
using UnityEngine;

namespace Amogh
{
    public class A11_ChargeAttack : MonoBehaviour, A11_IAttack
    {
        public GameObject boomShurikenPrefab;
        public GameObject bassChild;

        public float force = 10f;
        public float AttackCooldownTimer = 5f;
        
        private float timer = 10f;
        private ParticleSystem boomChildParticles;
        void Start()
        {
            boomChildParticles = bassChild.GetComponentInChildren<ParticleSystem>();
        }
        

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
        }
        
        IEnumerator SpawnBass(AudioSource shurikenClip)
        {
            boomChildParticles.Play();

            // Total time is 8 seconds
            // Start only after ~4 seconds is done (charge up sound)
            yield return new WaitForSeconds(4 - 1.3f);
            for (int i = 0; i < 7; ++i)
            {
                GameObject shuriken = Instantiate(boomShurikenPrefab,
                    bassChild.transform.position + bassChild.transform.forward, Quaternion.identity);

                var rb = shuriken.GetComponent<Rigidbody>();
                
                rb.velocity = bassChild.transform.forward * force;
                
                Destroy(shuriken, 3f);
                yield return new WaitForSeconds(0.6f);
            }
            
            boomChildParticles.Stop();
            
            // Wait until sound clip is done
            //while (shurikenClip.isPlaying)
            //{
                //yield return new WaitForFixedUpdate();
            //}

            bassChild.SetActive(false);
            
            timer = 0;
        }
        
        public void ButtonDown()
        {
            if (bassChild.activeSelf || (timer < AttackCooldownTimer))
                return;
            
            // Start charging up sound clip
            bassChild.SetActive(true);
            var shurikenClip = bassChild.GetComponent<AudioSource>();
            shurikenClip.time = 1.3f;
            shurikenClip.Play();

            StartCoroutine(SpawnBass(shurikenClip));
            
        }
        
        public void ButtonHeld()
        {
            
        }

        public void ButtonUp()
        {
            
        }
    }
}