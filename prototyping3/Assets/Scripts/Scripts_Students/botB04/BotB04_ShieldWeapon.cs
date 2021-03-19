using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BotB04.Controller
{
    [System.Serializable]
    public class BotB04_ShieldWeapon
    {
        [SerializeField]
        private GameObject weaponRef;
        [HideInInspector]
        public BotB04_Gun weapon;
        public RechargableShield shieldStats;

        public void Initialize()
        {
            weapon = weaponRef.GetComponent<BotB04_Gun>();
        }

        public bool RequestFire()
        {
            //If shields are recharging, can't fire
            if(shieldStats.power <= 0 || shieldStats.recharging)
            {
                weapon.Blank();
                return false;
            }

            weapon.Fire();
            return true;
        }

    }
}
