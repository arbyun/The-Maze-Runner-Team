using Systems;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool hasWeapon;
    public float attackDamage;

    /// <summary>
    /// If there's a weapon equipped, return the weapon. Used in the Damager script to Calculate Damage dealt
    /// </summary>
    /// <returns>Return the weapon object equipped</returns>
    public static GameObject GetWeapon()
    {
        GameObject weapon = InventorySystem.EquippedWeapon;
        return weapon;
    }
        
}