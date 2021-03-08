using System;
using System.Collections;
using UnityEngine;

namespace A09
{
    public class BotProjectile : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float range;
        [SerializeField] private float fallbackLifetime = 1.5f;

        [SerializeField] private Rigidbody body;
        [SerializeField] private HazardDamage damage;
        
        private float _lifetime = 0;
        private Vector3 _startingPos;

        public void SetTeam(bool isPlayer1)
        {
            damage.isPlayer1Weapon = isPlayer1;
            damage.isPlayer2Weapon = !isPlayer1;
        }

        private void OnValidate()
        {
            if (!damage)
                damage = GetComponent<HazardDamage>();
            
            if (!body)
                body = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            body.velocity = -transform.right * moveSpeed;
            _startingPos = transform.position;
        }

        private void Update()
        {
            if (Vector3.Distance(transform.localPosition, _startingPos) >= range)
                Destroy(gameObject);
            
            if (_lifetime >= fallbackLifetime)
                Destroy(gameObject);

            _lifetime += Time.deltaTime;
        }
    }
}