using System;
using System.Collections;
using GameSystems.Mechanics;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;


// REDONE: Using Unity's Input System now

namespace GameSystems
{
    public class InputManager : MonoBehaviour
    {
        public GameObject playerObject;
        private PlayerControls _controls;

        private Vector3 _currentPosition;
        
        /* Dashing Mechanics */

        #region Dash

        private float _speed = Player.Speed;
        public float dashTime = 2f;
        private bool _hasDashed;

        #endregion
        
        /* Jumping Mechanics */

        #region Jump

        private bool _isGrounded;
        private bool _isClimbing;
        private const float JumpHeight = 15.0f;
        private CharacterController _controller;
        private Vector3 _wallSurfNormal;
        private Vector3 _direction, _jumpingSpeed;
        private float _yVelocity;
        private bool _canWallJump;

        #endregion

        /// <summary> Method responsible for handling all input from the user.</summary>
        /// <returns> The current position of the player.</returns>
        public InputManager()
        {
            _currentPosition = playerObject.transform.position;
        }

        private void Awake()
        {
            _controls = new PlayerControls();
    
            _controls.Gameplay.MoveRight.performed += ctx => MoveRight();
            _controls.Gameplay.MoveLeft.performed += ctx => MoveLeft();
            _controls.Gameplay.Jump.performed += ctx => Jump();
            _controls.Gameplay.Attack.performed += ctx => Attack();
            _controls.Gameplay.Escape.performed += ctx => Escape();
            _controls.Gameplay.OpenInventory.performed += ctx => OpenInventory();
            _controls.Gameplay.SwitchWeapons.performed += ctx => SwitchWeapons();
            _controls.Gameplay.Dash.performed += ctx => Dash();
            _controls.Gameplay.ClimbUp.performed += ctx => ClimbUp();
            _controls.Gameplay.ClimbDown.performed += ctx => ClimbDown();

            if (!playerObject.TryGetComponent<Player>(out var player))
            {
                Debug.LogError("This is not our player.");
            }
        }

        private void Update()
        {
            _currentPosition = playerObject.transform.position;
        }

        private void OnEnable()
        {
            _controls.Gameplay.Enable();
        }
    
        private void OnDisable()
        {
            _controls.Gameplay.Disable();
        }

        private void MoveRight()
        {
            playerObject.transform.position += Vector3.right;
        }

        private void MoveLeft()
        {
            playerObject.transform.position += Vector3.left;
        }
    
        /// <summary> Responsible for the player's jumping mechanics.        
        /// If the player is grounded, they will jump straight up.
        /// If the player is climbing a wall, they can either climb up/down or wall jump off of it.
        /// Otherwise, if in midair, they will simply jump straight up.</summary>
        /// <returns> A vector3</returns>
        private void Jump()
        {
            if (_isGrounded)
            {
                // Jump from the ground
                _yVelocity = JumpHeight;
            }
            else if (_isClimbing)
            {
                if (_canWallJump)
                {
                    // Wall jump
                    _yVelocity = JumpHeight;
                    _jumpingSpeed = _wallSurfNormal * _speed;
                }
                else
                {
                    // Climb up/down
                    _yVelocity = Input.GetAxisRaw("Vertical") * _speed;
                    _jumpingSpeed = Vector3.zero;
                }
            }
            else
            {
                // Jump while in air
                //_yVelocity = JumpHeight;
                //_jumpingSpeed = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * _speed;
            }

            _canWallJump = false;
            _direction = _jumpingSpeed.normalized;
        }

    
        private void Attack()
        {
            AttackManager.Attack();
        }
    
        /// <summary> The Escape function is used to toggle the Escape Menu on and off.        
        /// &lt;para&gt;The function first finds the GameObject with the tag &quot;Escapemenu&quot; and stores it in a variable called esc.&lt;/para&gt;
        /// &lt;para&gt;If esc is active, then set it to inactive. If esc is not active, then set it to active.&lt;/para&gt;</summary>
        ///
        ///
        /// <returns> A boolean value. </returns>
        private void Escape()
        {
            GameObject esc = GameObject.FindWithTag("EscapeMenu");

            if (esc.activeSelf)
            {
                esc.SetActive(false);
            }
            else if (!esc.activeSelf)
            {
                esc.SetActive(true);
            }
        }
    
        /// <summary> The OpenInventory function is called when the player presses the &quot;I&quot; key.        
        /// It finds an object with a tag of &quot;Inventory&quot;, and if it's active, sets it to inactive.
        /// If it's not active, then set it to active.</summary>
        ///
        ///
        /// <returns> A gameobject</returns>
        private void OpenInventory()
        {
            GameObject inv = GameObject.FindWithTag("Inventory");

            if (inv.activeSelf)
            {
                inv.SetActive(false);
            }
            else if (!inv.activeSelf)
            {
                inv.SetActive(true);
            }
        }
    
        private void SwitchWeapons()
        {
            WeaponSwitchUI.SwitchWeapons();
        }
    
        /// <summary> Used to increase the player's speed for a short period of time.        
        /// The dashTime variable is used to determine how long the player will be able to move at an
        /// increased speed. The _hasDashed boolean is used as a flag that determines whether or not the cooldown
        /// has started.</summary>
        /// <returns> A boolean value, which is used to determine whether the player has dashed</returns>
        private void Dash()
        {
            float horizontalIn = Input.GetAxis("Horizontal");
            float verticalIn = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalIn, verticalIn, 0);
            
            // The actual dashing
            playerObject.transform.Translate(direction * (_speed * Time.deltaTime));

            // Timer here is not for cd, rather it is for the time window where the player's speed will be increased
            dashTime -= Time.deltaTime;
            
            if (dashTime <= 0)
            {
                _speed *= 1.5f;
                dashTime = 2f;
                _hasDashed = true;
            }
            else if (dashTime > 0 && _hasDashed)
            {
                // Now we start the actual cooldown
                StartCoroutine(AfterDash());
            }
        }
        
        private IEnumerator AfterDash()
        {
            // Let's return to the og speed
            _speed /= 1.5f;
            yield return new WaitForSeconds(1.2f);
        }
    
        /// <summary> Allows the player to climb up a wall if they are touching it.</summary>
        /// <returns> A vector2.</returns>
        private void ClimbUp()
        {
            if (playerObject.GetComponent<Collider2D>().IsTouchingLayers(Convert.ToInt32("Wall")))
            {
                _currentPosition.y += 1.0f;
                playerObject.transform.position = _currentPosition;
                _canWallJump = true;
            }
        }
    
        /// <summary> Allows the player to climb down a wall if they are touching it.        
        /// </summary>
        private void ClimbDown()
        {
            if (playerObject.GetComponent<Collider2D>().IsTouchingLayers(Convert.ToInt32("Wall")))
            {
                _currentPosition.y -= 1.0f;
                playerObject.transform.position = _currentPosition;
                _canWallJump = true;
            }
        }
    }
}