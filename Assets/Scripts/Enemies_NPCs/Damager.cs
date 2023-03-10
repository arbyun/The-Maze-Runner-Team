using UnityEngine;

namespace Enemies_NPCs
{
    /// <summary>
    /// This limits the type of objects that deal damage; Also useful so we don't keep recycling
    /// code for the same thing
    /// </summary>
    public class Damager : MonoBehaviour
    {
        public float damage;
        private bool _component;

        /// <summary>
        /// If the damager is a player or enemy, replace the damage serialised value for the ones
        /// set in their own script
        /// </summary>
        private void OnEnable()
        {
            if (_component == TryGetComponent<Player>(out Player player))
            {
                damage = player.attackDamage;
            }
            if (_component == TryGetComponent<Enemy>(out Enemy enemy))
            {
                damage = enemy.attackDamage;
            }
        }

        /// <summary>
        /// Useful and very simple way of communicating dmg between objects
        /// </summary>
        /// <returns>Damage dealt by the damager</returns>
        public float GetDamage()
        {
            damage = CalculateDamage();
            return damage; 
        }
        
        private float CalculateDamage()
        {
            // If the damager is the player and has a weapon equipped, we add the weapon's dmg to the total
            if (_component == TryGetComponent<Player>(out Player player))
            {
                if (player.hasWeapon)
                {
                    GameObject weaponPrefab = Player.GetWeapon();
                    Weapon weapon = weaponPrefab.GetComponent<Weapon>();
                    float weaponDmg = weapon.attackModifier;
                    damage += weaponDmg;
                }
            }

            return damage;
        }
    }
}