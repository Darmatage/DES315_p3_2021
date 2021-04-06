using System.Collections;
using UnityEngine;

namespace Amogh
{
    public class A11_ChargeAttack : MonoBehaviour, A11_IAttack
    {
        public GameObject boomShurikenPrefab;
        public GameObject bassChild;

        public float force = 10f;
        public float AttackCooldownTimer = 3f;
        
        private float timer = 10f;
        private ParticleSystem boomChildParticles;
        private Animator bassAnimator;

        private Vector3 target;
        private bool useTarget;
        void Awake()
        {
            boomChildParticles = bassChild.GetComponentInChildren<ParticleSystem>();
            bassAnimator = bassChild.GetComponent<Animator>();
            
            bassChild.SetActive(true);
        }
        

        // Update is called once per frame
        void Update()
        {
        }
        
        IEnumerator ReActivate()
        {
            while (timer < AttackCooldownTimer)
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            bassChild.SetActive(true);
        }
        
        IEnumerator Cooldown()
        {
            while (timer < AttackCooldownTimer)
            {
                bassChild.transform.localScale = Vector3.Lerp(bassChild.transform.localScale, Vector3.one,
                    timer / AttackCooldownTimer);
                
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        IEnumerator SpawnBass(AudioSource shurikenClip)
        {
            boomChildParticles.Play();

            // Total time is 8 seconds
            // Start only after ~4 seconds is done (charge up sound)
            yield return new WaitForSeconds(4 - 1.3f);
            
            for (int i = 0; i < 4; ++i)
            {
                GameObject shuriken = Instantiate(boomShurikenPrefab,
                    bassChild.transform.position + bassChild.transform.forward, Quaternion.identity);
                
                shuriken.SetActive(true);
                var rb = shuriken.GetComponent<Rigidbody>();

                if (useTarget == false)
                {
                    rb.velocity = bassChild.transform.forward * force;
                }
                else
                {
                    rb.velocity = (target - bassChild.transform.position).normalized * force;
                }
                
                Destroy(shuriken, 3f);
                yield return new WaitForSeconds(0.5f);
            }

            // Wait until sound clip is done
            //while (shurikenClip.isPlaying)
            //{
                //yield return new WaitForFixedUpdate();
            //}
            
            shurikenClip.Stop();
            boomChildParticles.Stop();
            bassAnimator.SetBool("Charging", false);
            bassChild.SetActive(false);
            //StartCoroutine(Cooldown());
            StartCoroutine(ReActivate());
        }
        
        public void ButtonDown()
        {
            if (timer <= AttackCooldownTimer)
                return;
            
            // Start charging up sound clip
            bassChild.SetActive(true);
            var shurikenClip = bassChild.GetComponent<AudioSource>();
            shurikenClip.time = 1.3f;
            shurikenClip.Play();
            
            bassAnimator.SetBool("Charging", true);
            
            StartCoroutine(SpawnBass(shurikenClip));
            timer = 0;
        }
        
        public void ButtonHeld()
        {
            
        }

        public void ButtonUp()
        {
            
        }

        public void SetTarget(Vector3 pos)
        {
            target = pos;
            useTarget = true;
        }
    }
}