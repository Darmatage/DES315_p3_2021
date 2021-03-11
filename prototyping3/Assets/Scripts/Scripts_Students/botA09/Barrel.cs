using System.Collections;
using UnityEngine;

namespace A09
{
    public class Barrel : MonoBehaviour
    {
        [SerializeField] private Vector3 projectileSpawnOffset;
        [SerializeField] private BotProjectile projectilePrefab;
        [SerializeField] private float cooldown;
        
        private bool _onCooldown = false;
        
        public void Fire()
        {
            if (_onCooldown)
                return;
            
            var projectileInstance = Instantiate(projectilePrefab, transform.TransformPoint(transform.localPosition + projectileSpawnOffset),
                Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90, 0)));

            projectileInstance.SetTeam(gameObject.transform.root.tag == "Player1");
            
            StartCoroutine(DoCooldown());
        }

        private IEnumerator DoCooldown()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(cooldown);
            _onCooldown = false;
        }
    }
}