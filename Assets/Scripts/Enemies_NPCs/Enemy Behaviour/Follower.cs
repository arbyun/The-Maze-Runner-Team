using UnityEngine;

namespace Enemies_NPCs.Enemy_Behaviour
{
    public class Follower : Enemy
    {
        [SerializeField] private float speed;
        private float _visionRange;
        private float _attackRange;
        private GameObject _player;
        private Transform _playerT;
        private bool _isFollowing;
        private bool _isAttacking;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _playerT = _player.transform;
            _isAttacking = false;
        }

        /// <summary>
        /// This function is here for easy animating purposes
        /// </summary>
        private static void HasSeenPlayer()
        {
            return;
        }

        private void Update()
        {
            // Let's get the distance to the player
            float distance = Vector3.Distance(transform.position, _playerT.position);
            
            // If player is in our vision range, start following them
            if (distance <= _visionRange)
            {
                HasSeenPlayer();
                _isFollowing = true;
            }

            // If player is in our attack range, attack
            if (distance <= _attackRange)
            {
                if (_isAttacking)
                {
                    StartCoroutine(DealDamage());
                    _isAttacking = false;
                }
            }

            // Follow the player
            if (_isFollowing)
            {
                transform.LookAt(_playerT.position);
                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            }
        }
    }
}
