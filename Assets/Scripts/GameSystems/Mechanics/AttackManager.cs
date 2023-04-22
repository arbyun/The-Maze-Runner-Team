using System;
using Enemies_NPCs;
using UnityEngine;

namespace GameSystems.Mechanics
{
    public class AttackManager : MonoBehaviour
    {
        public GameObject player;
        [SerializeField] private static float AttackCooldown;

        private static float _currentCooldown;

        private void Start()
        {
            _currentCooldown = 0;
        }

        /// <summary> The Attack function is called when the player presses the attack button.        
        /// It checks if there is a weapon equipped, and if so, plays its firing animation.
        /// If there are no weapons equipped, it does nothing.</summary>
        /// <returns> Nothing</returns>
        internal static void Attack()
        {
            if (_currentCooldown == 0)
            {
                GameObject wpn = Player.GetWeapon();
                Weapon weapon = wpn.GetComponent<Weapon>();

                if (weapon.HasChildren)
                {
                    Animation anim = weapon.children.GetComponent<Animation>();
                    anim.Play();
                }
                else
                {
                    weapon.firing.Play();
                }

                _currentCooldown = AttackCooldown;
            }
            
        }

        private void Update()
        {
            _currentCooldown -= Time.deltaTime;

            if (_currentCooldown < 0)
            {
                _currentCooldown = 0;
            }
        }
    }
}