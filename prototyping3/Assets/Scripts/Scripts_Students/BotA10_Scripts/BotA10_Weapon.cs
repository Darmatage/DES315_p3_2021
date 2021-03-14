
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class BotA10_Weapon : MonoBehaviour
    {
        //NOTE: This script goes on the main playerBot Game Object, and the weapon goes in the public GO slot
        [SerializeField] private float mainGunCooldownTimerMax = 1f;
        private float _mainGunCooldownTimer = 1f;
        
        [SerializeField] private Transform bulletPrefab = default;
        [SerializeField] private Transform bulletSpawnPoint = default;
        [SerializeField] private GameObject rightFlameThrower = default;
        [SerializeField] private GameObject leftFlameThrower = default;
        
        //grab axis from parent object
        public string button1;
        public string button2;
        public string button3;
        public string button4; // currently boost in player move script

        //public event EventHandler OnShootB1;

        public void Start()
        {
            button1 = gameObject.transform.parent.GetComponent<playerParent>().action1Input;
            button2 = gameObject.transform.parent.GetComponent<playerParent>().action2Input;
            button3 = gameObject.transform.parent.GetComponent<playerParent>().action3Input;
            button4 = gameObject.transform.parent.GetComponent<playerParent>().action4Input;

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
            // instantiate the bullet
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<BotA10_Bullet>().Setup(transform.forward);
            
            // set team for bullet
            if (gameObject.transform.root.tag == "Player1") { bullet.GetComponent<HazardDamage>().isPlayer1Weapon = true; }
            else { bullet.GetComponent<HazardDamage>().isPlayer2Weapon = true; }
        }
        
    }
}