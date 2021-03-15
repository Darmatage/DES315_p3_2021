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
            GameObject.Instantiate(bullet, bulletSpawn.transform);
            bullet.transform.parent = null;
        }



    }
}