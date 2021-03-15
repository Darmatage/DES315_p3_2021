
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class BotA10_Bullet : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 100f;
        private Rigidbody _rigidbody1;

        private void Awake()
        {
            _rigidbody1 = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            Destroy(gameObject, 0.01f);
        }
        
        public void Setup(Vector3 dir)
        {
            _rigidbody1.AddForce(dir * moveSpeed, ForceMode.Impulse);
        }
    }
}