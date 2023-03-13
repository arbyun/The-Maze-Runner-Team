using System.Collections;
using UnityEngine;

namespace Enemies_NPCs
{
    /// <summary>
    /// Default class for the enemy; all enemy types share these properties and functions
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    public class Enemy : Damager
    {
        private ETypes _enemyType;
        // [0] should be current health, [1] should be max health
        public int[] healthMinMax;
        private float _attackDamage;
        private bool _isDead;
        public float attackCooldown;
        private Collider2D _cd2D;
        private Rigidbody2D _rb2D;
        private Animator _anim;
        public Animation deathAnimation;
        public Animation attackAnimation;

        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _cd2D = GetComponent<Collider2D>();
            _anim = GetComponent<Animator>();
            _attackDamage = GetComponent<Damager>().damage;
        }

        /// <summary>
        /// If we collide with something that has the ability of dealing damage to us
        /// then let's ask it for it's damage and apply it to us
        /// </summary>
        /// <param name="other">Object that collides with us</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag($"EnemyDamager") | other.CompareTag($"GeneralDamager"))
            {
                float dmg = other.transform.GetComponent<Damager>().GetDamage();
                TakeDamage(dmg);
            }
        }

        private void TakeDamage(float dmg)
        {
            // Take the damage amount off our current health
            healthMinMax[0] = (int)Mathf.Clamp(healthMinMax[0] - dmg, 0, healthMinMax[1]);

            if (healthMinMax[0] <= 0)
            {
                _isDead = true;
            }

            if (_isDead)
            {
                deathAnimation.Play();
                _cd2D.enabled = false; // Let's disable this to not get any bugs with enemies mass blocking the way
                _rb2D.Sleep();
            }
        }

        /// <summary>
        /// This is here mostly for managing the animation and cooldown,
        /// the real DealDamage is already in the Damager script
        /// </summary>
        /// <returns>Cooldown until it can attack again</returns>
        internal IEnumerator DealDamage()
        {
            if (_attackDamage != 0)
            {
                attackAnimation.Play();
                yield return new WaitForSecondsRealtime(attackCooldown);
            }
        }
    }
}
