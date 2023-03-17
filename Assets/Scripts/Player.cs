using Enemies_NPCs;
using GameSystems;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Player : Damager
{
    internal bool HasWeapon;
    public int[] healthMinMax;
    private bool _isDead;
    public float attackCooldown;
    private Collider2D _cd2D;
    private Rigidbody2D _rb2D;
    private Animator _anim;
    public float speed;

    private void Start()
    {
        _cd2D = GetComponent<Collider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

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