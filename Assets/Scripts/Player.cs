/*using System;
using Enemies_NPCs;
using GameSystems;
using UI;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]

public class Player : Damager
{
    internal bool HasWeapon;
    public bool TutorialDone;
    private bool _isDead;
    
    public int[] HealthMinMax;
    public float AttackCooldown;
    public static float Speed;
    
    private Collider2D _cd2D;
    private Rigidbody2D _rb2D;
    private Animator _anim;
    private Configs _configs;
    private PlayerControls _controls;
    
    /* ---------------------------------------------------- 
    #region Player Enums

    public enum Mode
    {
        Normal,
        Climbing
    }
    
    public enum Direction
    {
        Left = -1,
        Right = 1
    }
    
    public enum AttackMode
    {
        Normal,
        Dash,
        Air,
        DashAir
    }

    public static event Action OnInteract;

    #endregion

    /* --------------------------------------------------- 
    #region Health System

    private static int _health;

    public static int CurrentHealth
    {
        get => _health;

        set => _health = Mathf.Min(value, MaxHealth);
    }

    public static int MaxHealth;
    
    #endregion
    
    /* --------------------------------------------------- 
    #region Movement Vectors
    public Vector2 Position => transform.position; 
    private Vector2 _hazardRespawnPoint = new Vector2(0, 0);
    private Vector2 _deathRespawnPoint = new Vector2(0, 0);

    #endregion
    
    /* --------------------------------------------------- 
    /// <summary> Static function that returns the singleton instance of the Player class.</summary>    
    /// <returns> A new instance of the player class</returns>
    private Player()
    {
        MaxHealth = HealthMinMax[1];
    }

    /// <summary> Used to initialize any variables or game state before the game starts.</summary>    
    /// <returns> The values of the player physics.</returns>
    private void Awake()
    {
        _configs = Configs.Load();
        _controls = new PlayerControls();
        
        if (_configs != null)
        {
            if (_configs.playerPhysics.gravity != 0)
            {
                _rb2D.gravityScale = _configs.playerPhysics.gravity;
                //_gravity = _configs.playerPhysics.gravity;
            }

            if (_configs.playerPhysics.mass != 0)
                _rb2D.mass = _configs.playerPhysics.mass;
                
            if (_configs.playerPhysics.linearDrag != 0)
                _rb2D.drag = _configs.playerPhysics.linearDrag;

            if (_configs.playerPhysics.angularDrag != 0)
                _rb2D.angularDrag = _configs.playerPhysics.angularDrag;

        }
    }

    /// <summary> Set up the player's speed, collider, rigidbody and animator components.
    /// It also enables controls for the player.</summary>
    /// <returns> Nothing</returns>
    private void Start()
    {
        Speed = 10;
        
        _cd2D = GetComponent<Collider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        
        _controls.Enable();
        
        if (!TutorialDone)
        { 
            InventorySystem.AddItem(GameObject.Find("TutorialGun"));
            InventorySystem.AddItem(GameObject.Find("TutorialKnife"));
            InventorySystem.AddItem(GameObject.Find("Cherry"));
            InventorySystem.EquipWeapon(GameObject.Find("TutorialKnife"));
            InventorySystem.OffHandWeapon(GameObject.Find("TutorialGun"));
        }
        
    }

    /// <summary>
    /// If there's a weapon equipped, return the weapon. Used in the Damager script to Calculate Damage dealt.
    /// </summary>
    /// <returns>Return the weapon object equipped</returns>
    public static GameObject GetWeapon() => InventorySystem.EquippedWeapon;

    private void Update()
    {
        if (_health <= 0)
            Respawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"PlayerDamager") || other.CompareTag($"GeneralDamager"))
        {
            float dmg = other.transform.GetComponent<Damager>().GetDamage();
            TakeDamage(dmg);
        }
    }

    /// <summary> Takes a float as an argument and subtracts it from the player's current health.    
    /// If the player's health is less than or equal to 0, then they are respawned at their last checkpoint.</summary>
    /// <param name="dmg"> The amount of damage to take</param>
    private void TakeDamage(float dmg)
    {
        // Take the damage amount off our current health
        HealthMinMax[0] = (int)Mathf.Clamp(HealthMinMax[0] - dmg, 0, HealthMinMax[1]);

        if (_health <= 0)
            Respawn();
        
        StartCoroutine(DisplayHUD.FlashWhite(DisplayHUD.HealthBarImage));
    }
    
    /// <summary> Resets the player's position to a predefined respawn point, and sets their health
    /// back to full.</summary>
    /// <returns> The position of the player.</returns>
    private void Respawn()
    {
        transform.position = _deathRespawnPoint;
        _health = MaxHealth;
        Speed = 10;
    }
        
}*/