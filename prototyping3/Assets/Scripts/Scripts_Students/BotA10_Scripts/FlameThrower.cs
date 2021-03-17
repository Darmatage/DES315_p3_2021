
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class FlameThrower : MonoBehaviour
    {
        public string player;
        public float damage = 1f;
        private GameHandler _gameHandler;
        public bool isPlayer1Weapon = false;
        public bool isPlayer2Weapon = false;

    
        [SerializeField] private float cooldownTimerMax = 1f;
        private float _cooldownTimer = 1f;
    
        void Start()
        {
            if (GameObject.FindWithTag("GameHandler") != null)
            {
                _gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
            }

            if (gameObject.transform.root.CompareTag("Player1")) { isPlayer1Weapon = true; }
            if (gameObject.transform.root.CompareTag("Player2")) { isPlayer2Weapon = true; }
        }

        private void Update()
        {
            if (_cooldownTimer > 0) { _cooldownTimer -= Time.deltaTime; }
            if (_cooldownTimer < 0) { _cooldownTimer = 0; }
        }

        void OnTriggerStay(Collider other)
        {
            string target = other.gameObject.transform.root.tag;
            if (_gameHandler != null && target != player)
            {
                if (target == "Player1")
                {
                    if (_cooldownTimer == 0)
                    {
                        _gameHandler.TakeDamage("Player1", damage);
                        _cooldownTimer = cooldownTimerMax;
                    }
                }
                if (target == "Player2")
                {
                    if (_cooldownTimer == 0)
                    {
                        _gameHandler.TakeDamage("Player2", damage);
                        _cooldownTimer = cooldownTimerMax;
                    }
                }
            }
        }
    }
}
