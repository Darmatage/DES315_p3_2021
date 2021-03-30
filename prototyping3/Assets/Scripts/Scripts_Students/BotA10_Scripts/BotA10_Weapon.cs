
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class BotA10_Weapon : MonoBehaviour
    {
        // Timers
        [SerializeField] private float mainGunCooldownTimerMax = 1f;
        private float _mainGunCooldownTimer = 1f;
        
        // Objects
        [SerializeField] private Transform bulletPrefab = default;
        [SerializeField] private Transform bulletSpawnPoint = default;
        [SerializeField] private GameObject rightFlameThrower = default;
        [SerializeField] private GameObject leftFlameThrower = default;
        
        // Audio
        private AudioSource _audioSource;
        
        // buttons
        public string button1;
        public string button2;
        public string button3;

        public void Start()
        {
            // button setup
            var parent = gameObject.transform.parent.GetComponent<playerParent>();
            button1 = parent.action1Input;
            button2 = parent.action2Input;
            button3 = parent.action3Input;
            // audio setup
            _audioSource = GetComponent<AudioSource>();
            // timer setup
            _mainGunCooldownTimer = mainGunCooldownTimerMax;
        }

        private void Update()
        {
            // main gun
            if (_mainGunCooldownTimer > 0) { _mainGunCooldownTimer -= Time.deltaTime; }
            if (_mainGunCooldownTimer < 0) { _mainGunCooldownTimer = 0; }
            if (Input.GetButtonDown(button1) && _mainGunCooldownTimer == 0)
            {
                ShootMainGun();
                _mainGunCooldownTimer = mainGunCooldownTimerMax;
            }
            // right flamethrower
            if (Input.GetButtonDown(button2))
            {
                rightFlameThrower.SetActive(true);
            }
            if (Input.GetButtonUp(button2))
            {
                rightFlameThrower.SetActive(false);
            }
            // left flamethrower
            if (Input.GetButtonDown(button3))
            {                                
                leftFlameThrower.SetActive(true);
            }
            if (Input.GetButtonUp(button3))
            {
                leftFlameThrower.SetActive(false);
            }                                  
        }


        private void ShootMainGun()
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<BotA10_Bullet>().Setup(transform.forward);
            
            _audioSource.Play();
            
            if (gameObject.transform.root.tag == "Player1") { bullet.GetComponent<HazardDamage>().isPlayer1Weapon = true; }
            else { bullet.GetComponent<HazardDamage>().isPlayer2Weapon = true; }
        }
        
    }
}