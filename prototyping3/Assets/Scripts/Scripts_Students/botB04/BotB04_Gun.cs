using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BotB04.Controller
{
    public class BotB04_Gun : MonoBehaviour
    {
        public GameObject bullet;
        public GameObject bulletSpawn;


        public void Fire()
        {
            GameObject asd = GameObject.Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);

            HazardDamage hazardAttr = asd.GetComponent<HazardDamage>();

            if (gameObject.transform.root.tag == "Player1") { hazardAttr.isPlayer1Weapon = true; }
            if (gameObject.transform.root.tag == "Player2") { hazardAttr.isPlayer2Weapon = true; }

        }
    }
}