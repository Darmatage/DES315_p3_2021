using System;
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class BotA10_ShootProjectiles : MonoBehaviour
    {
        private BotA10_Weapon weapon;
        [SerializeField] private Transform bulletPrefab = default;
        [SerializeField] private Transform bulletSpawnPoint = default;
        private void Awake()
        {
            weapon = GetComponent<BotA10_Weapon>();
            weapon.OnShootB1 += BotA10_Shoot;
        }

        private void BotA10_Shoot(object sender, EventArgs e)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<BotA10_Bullet>().Setup(transform.forward);
            if (gameObject.transform.root.tag == "Player1") { bullet.GetComponent<HazardDamage>().isPlayer1Weapon = true; }
            else { bullet.GetComponent<HazardDamage>().isPlayer2Weapon = true; }
        }
    }
}
