﻿
using System;
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class BotA10_Bullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 100f;
        private bool isPlayer1;
        private Rigidbody rigidbody1;

        private void Awake()
        {
            rigidbody1 = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            Destroy(gameObject, 0.01f);
        }
        
        public void Setup(Vector3 dir)
        {
            rigidbody1.AddForce(dir * moveSpeed, ForceMode.Impulse);
        }
    }
}