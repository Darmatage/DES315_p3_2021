
using UnityEngine;

namespace Scripts_Students.BotA10_Scripts
{
    public class FlameThrower : MonoBehaviour
    {
        public string player;
        public bool isPlayer1Weapon = false;
        public bool isPlayer2Weapon = false;

        [SerializeField] private float damage = 1f;
        [SerializeField] private float cooldownTimerMax = 1f;
        private float _cooldownTimer = 1f;
        private GameHandler _gameHandler;

        private void Start()
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
            if (_cooldownTimer < 0) { _cooldownTimer = 0; damage = 1f; }
        }
        
        
        /*
         * I couldn't figure out a way to make the flamethrower
         * do damage without ignoring shields. Thus, I ignored them.
         * Does little damage overtime anyway.
         */
        private void OnTriggerStay(Collider other)
        {
            var target = other.gameObject.transform.root.tag;
            if (_gameHandler == null || target == player) return;
            switch (target)
            { 
                case "Player1":
                    if (_cooldownTimer == 0)
                    {
                        _gameHandler.TakeDamage("Player1", damage);
                        damage = 0f; // set to 0 to fix too much damage issue
                        _cooldownTimer = cooldownTimerMax;
                    }
                    break;
                case "Player2":
                    if (_cooldownTimer == 0)
                    {
                        _gameHandler.TakeDamage("Player2", damage);
                        damage = 0f; // set to 0 to fix too much damage issue
                        _cooldownTimer = cooldownTimerMax;
                    } 
                    break;
            }
        }
    }
}
