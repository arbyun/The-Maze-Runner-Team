using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies_NPCs
{
    /// <summary>
    /// Default class for the enemy; all enemy types share these properties and functions
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        private ETypes _enemyType;
        // [0] should be current health, [1] should be max health
        private int[] _healthMinMax;
        [FormerlySerializedAs("_attackDamage")] public float attackDamage;
        private bool _isDead;
        public float attackCooldown;
        public Collider2D cd2D;
        public Rigidbody2D rb2D;
        public Animator anim;
        public Animation deadAnimation;
        public Animation attackAnimation;

        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            cd2D = GetComponent<Collider2D>();
            anim = GetComponent<Animator>();
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
                float damage = other.transform.GetComponent<Damager>().GetDamage();
                TakeDamage(damage);
            }
        }

        private void TakeDamage(float dmg)
        {
            // Take the damage amount off our current health
            _healthMinMax[0] = (int)Mathf.Clamp(_healthMinMax[0] - dmg, 0, _healthMinMax[1]);

            if (_healthMinMax[0] <= 0)
            {
                _isDead = true;
            }

            if (_isDead)
            {
                deadAnimation.Play();
                cd2D.enabled = false; // Let's disable this to not get any bugs with enemies mass blocking the way
                rb2D.Sleep();
            }
        }

        /// <summary>
        /// This is here mostly for managing the animation and cooldown,
        /// the real DealDamage is already in the Damager script
        /// </summary>
        /// <returns>Cooldown until it can attack again</returns>
        internal IEnumerator DealDamage()
        {
            if (attackDamage != 0)
            {
                attackAnimation.Play();
                yield return new WaitForSecondsRealtime(attackCooldown);
            }
        }
    }
}
