using System;
using System.Collections;
using Enemies_NPCs.Enemy_Behaviour;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Base Variables
        
        public int health;
        public float moveSpeed;
        public float jumpSpeed;
        public int jumpLeft;
        public Vector2 climbJumpForce;
        public float fallSpeed;
        public float sprintSpeed;
        public float sprintTime;
        public float sprintInterval;
        public float attackInterval;
        
        #endregion

        #region Health Variables
        
        public Color invulnerableColor;
        public Vector2 hurtRecoil;
        public float hurtTime;
        public float hurtRecoverTime;
        public Vector2 deathRecoil;
        public float deathDelay;
        
        #endregion

        #region Attack Variables
        
        public Vector2 attackUpRecoil;
        public Vector2 attackForwardRecoil;
        public Vector2 attackDownRecoil;

        public GameObject attackUpEffect;
        public GameObject attackForwardEffect;
        public GameObject attackDownEffect;

        #endregion

        #region State Bools
        
        private bool _isGrounded;
        private bool _isClimb;
        private bool _isSprintable;
        private bool _isSprintReset;
        private bool _isInputEnabled;
        private bool _isFalling;
        private bool _isAttackable;

        #endregion

        #region Time Variables
        
        private float _climbJumpDelay = 0.2f;
        private float _attackEffectLifeTime = 0.05f;
        
        #endregion

        #region Components
        
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;
        public PlayerInput playerInput;

        #endregion

        private void Start() {
            _isInputEnabled = true;
            _isSprintReset = true;
            _isAttackable = true;

            _animator = gameObject.GetComponent<Animator>();
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _transform = gameObject.GetComponent<Transform>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            UpdatePlayerState();
            if (_isInputEnabled)
            {
                Move();
                JumpControl();
                FallControl();
                SprintControl();
                AttackControl();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // enter climb state
            if (collision.collider.CompareTag("Wall") && !_isGrounded)
            {
                _rigidbody.gravityScale = 0;

                Vector2 newVelocity;
                newVelocity.x = 0;
                newVelocity.y = -2;

                _rigidbody.velocity = newVelocity;

                _isClimb = true;
                _animator.SetBool("IsClimb", true);

                _isSprintable = true;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Wall") && _isFalling && !_isClimb)
            {
                OnCollisionEnter2D(collision);
            }
        }

        public void Hurt(int damage)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");

            health = Math.Max(health - damage, 0);

            if (health == 0)
            {
                Die();
                return;
            }

            // enter invulnerable state
            _animator.SetTrigger("IsHurt");

            // stop player movement
            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = 0;
            _rigidbody.velocity = newVelocity;

            // visual effect
            _spriteRenderer.color = invulnerableColor;

            // death recoil
            Vector2 newForce;
            newForce.x = -_transform.localScale.x * hurtRecoil.x;
            newForce.y = hurtRecoil.y;
            _rigidbody.AddForce(newForce, ForceMode2D.Impulse);

            _isInputEnabled = false;

            StartCoroutine(RecoverFromHurtCoroutine());
        }

        private IEnumerator RecoverFromHurtCoroutine()
        {
            yield return new WaitForSeconds(hurtTime);
            _isInputEnabled = true;
            yield return new WaitForSeconds(hurtRecoverTime);
            _spriteRenderer.color = Color.white;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            // exit climb state
            if (collision.collider.CompareTag("Wall"))
            {
                _isClimb = false;
                _animator.SetBool("IsClimb", false);

                _rigidbody.gravityScale = 1;
            }
        }

        /* ######################################################### */

        private void UpdatePlayerState()
        {
            _isGrounded = CheckGrounded();
            _animator.SetBool("IsGround", _isGrounded);

            float verticalVelocity = _rigidbody.velocity.y;
            _animator.SetBool("IsDown", verticalVelocity < 0);

            if (_isGrounded && verticalVelocity == 0)
            {
                _animator.SetBool("IsJump", false);
                _animator.ResetTrigger("IsJumpFirst");
                _animator.ResetTrigger("IsJumpSecond");
                _animator.SetBool("IsDown", false);

                jumpLeft = 2;
                _isClimb = false;
                _isSprintable = true;
            }
            else if(_isClimb)
            {
                // one remaining jump chance after climbing
                jumpLeft = 1;
            }
        }

        private void Move()
        {
            // calculate movement
            float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

            // set velocity
            Vector2 newVelocity;
            newVelocity.x = horizontalMovement;
            newVelocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = newVelocity;

            if (!_isClimb)
            {
                // the sprite itself is inversed 
                float moveDirection = -transform.localScale.x * horizontalMovement;

                if (moveDirection < 0)
                {
                    // flip player sprite
                    Vector3 newScale;
                    newScale.x = horizontalMovement < 0 ? 1 : -1;
                    newScale.y = 1;
                    newScale.z = 1;

                    transform.localScale = newScale;

                    if (_isGrounded)
                    {
                        // turn back animation
                        _animator.SetTrigger("IsRotate");
                    }
                }
                else if (moveDirection > 0)
                {
                    // move forward
                    _animator.SetBool("IsRun", true);
                }
            }

            // stop
            if (Input.GetAxis("Horizontal") == 0)
            {
                _animator.SetTrigger("stopTrigger");
                _animator.ResetTrigger("IsRotate");
                _animator.SetBool("IsRun", false);
            }
            else
            {
                _animator.ResetTrigger("stopTrigger");
            }
        }

        private void JumpControl()
        {
            if (!Input.GetKeyDown(playerInput.Jump))
                return;

            if (_isClimb)
                ClimbJump();
            else if (jumpLeft > 0)
                Jump();
        }

        private void FallControl()
        {
            if (Input.GetKeyUp(playerInput.Jump) && !_isClimb)
            {
                _isFalling = true;
                Fall();
            } else
            {
                _isFalling = false;
            }
        }

        private void SprintControl()
        {
            if (Input.GetKeyDown(playerInput.Dash) && _isSprintable && _isSprintReset)
                Sprint();
        }

        private void AttackControl()
        {
            if (Input.GetKeyDown(playerInput.Attack) && !_isClimb && _isAttackable)
                Attack();
        }

        private void Die()
        {
            _animator.SetTrigger("IsDead");

            _isInputEnabled = false;

            // stop player movement
            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = 0;
            _rigidbody.velocity = newVelocity;

            // visual effect
            _spriteRenderer.color = invulnerableColor;

            // death recoil
            Vector2 newForce;
            newForce.x = -_transform.localScale.x * deathRecoil.x;
            newForce.y = deathRecoil.y;
            _rigidbody.AddForce(newForce, ForceMode2D.Impulse);

            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine()
        {
            var material = _boxCollider.sharedMaterial;
            material.bounciness = 0.3f;
            material.friction = 0.3f;
            // unity bug, need to disable and then enable to make it work
            _boxCollider.enabled = false;
            _boxCollider.enabled = true;

            yield return new WaitForSeconds(deathDelay);

            material.bounciness = 0;
            material.friction = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /* ######################################################### */

        private bool CheckGrounded()
        {
            Vector2 origin = _transform.position;

            float radius = 0.2f;

            // detect downwards
            Vector2 direction;
            direction.x = 0;
            direction.y = -1;

            float distance = 0.5f;
            LayerMask layerMask = LayerMask.GetMask("Platform");

            RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
            return hitRec.collider != null;
        }

        private void Jump()
        {
            Vector2 newVelocity;
            newVelocity.x = _rigidbody.velocity.x;
            newVelocity.y = jumpSpeed;

            _rigidbody.velocity = newVelocity;

            _animator.SetBool("IsJump", true);
            jumpLeft -= 1;
            if (jumpLeft == 0)
            {
                _animator.SetTrigger("IsJumpSecond");
            } 
            else if (jumpLeft == 1)
            {
                _animator.SetTrigger("IsJumpFirst");
            }
        }

        private void ClimbJump()
        {
            Vector2 realClimbJumpForce;
            realClimbJumpForce.x = climbJumpForce.x * transform.localScale.x;
            realClimbJumpForce.y = climbJumpForce.y;
            _rigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

            _animator.SetTrigger("IsClimbJump");
            _animator.SetTrigger("IsJumpFirst");

            _isInputEnabled = false;
            StartCoroutine(ClimbJumpCoroutine(_climbJumpDelay));
        }

        private IEnumerator ClimbJumpCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            _isInputEnabled = true;

            _animator.ResetTrigger("IsClimbJump");

            // jump to the opposite direction
            Vector3 newScale;
            newScale.x = -transform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            transform.localScale = newScale;
        }

        private void Fall()
        {
            Vector2 newVelocity;
            newVelocity.x = _rigidbody.velocity.x;
            newVelocity.y = -fallSpeed;

            _rigidbody.velocity = newVelocity;
        }

        private void Sprint()
        {
            // reject input during sprinting
            _isInputEnabled = false;
            _isSprintable = false;
            _isSprintReset = false;

            Vector2 newVelocity;
            newVelocity.x = transform.localScale.x * (_isClimb ? sprintSpeed : -sprintSpeed);
            newVelocity.y = 0;

            _rigidbody.velocity = newVelocity;

            if (_isClimb)
            {
                // sprint to the opposite direction
                Vector3 newScale;
                newScale.x = -transform.localScale.x;
                newScale.y = 1;
                newScale.z = 1;

                transform.localScale = newScale;
            }

            _animator.SetTrigger("IsSprint");
            StartCoroutine(SprintCoroutine(sprintTime, sprintInterval));
        }

        private IEnumerator SprintCoroutine(float sprintDelay, float sprintInterval)
        {
            yield return new WaitForSeconds(sprintDelay);
            _isInputEnabled = true;
            _isSprintable = true;

            yield return new WaitForSeconds(sprintInterval);
            _isSprintReset = true;
        }

        private void Attack()
        {
            float verticalDirection = Input.GetAxis("Vertical");
            if (verticalDirection > 0)
                AttackUp();
            else if (verticalDirection < 0 && !_isGrounded)
                AttackDown();
            else
                AttackForward();
        }

        private void AttackUp()
        {
            _animator.SetTrigger("IsAttackUp");
            attackUpEffect.SetActive(true);

            Vector2 detectDirection;
            detectDirection.x = 0;
            detectDirection.y = 1;

            StartCoroutine(AttackCoroutine(attackUpEffect, _attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
        }

        private void AttackForward()
        {
            _animator.SetTrigger("IsAttack");
            attackForwardEffect.SetActive(true);

            Vector2 detectDirection;
            detectDirection.x = -transform.localScale.x;
            detectDirection.y = 0;

            Vector2 recoil;
            recoil.x = transform.localScale.x > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
            recoil.y = attackForwardRecoil.y;

            StartCoroutine(AttackCoroutine(attackForwardEffect, _attackEffectLifeTime, attackInterval, detectDirection, recoil));
        }

        private void AttackDown()
        {
            _animator.SetTrigger("IsAttackDown");
            attackDownEffect.SetActive(true);

            Vector2 detectDirection;
            detectDirection.x = 0;
            detectDirection.y = -1;

            StartCoroutine(AttackCoroutine(attackDownEffect, _attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
        }

        private IEnumerator AttackCoroutine(GameObject attackEffect,float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
        {
            Vector2 origin = _transform.position;

            float radius = 0.6f;

            float distance = 1.5f;
            LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Trap") | LayerMask.GetMask("Switch") | LayerMask.GetMask("Projectile");

            RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

            foreach (RaycastHit2D hitRec in hitRecList)
            {
                GameObject obj = hitRec.collider.gameObject;

                string layerName = LayerMask.LayerToName(obj.layer);
            
                if (layerName == "Switch")
                {
                    Switch.Switcher swithComponent = obj.GetComponent<Switch.Switcher>();
                    if (swithComponent != null)
                        swithComponent.TurnOn();
                } 
                else if (layerName == "Enemy")
                {
                    Enemy enemyController = obj.GetComponent<Enemy>();
                    if (enemyController != null)
                        enemyController.Hurt(1);
                }
                else if (layerName == "Projectile")
                {
                    Destroy(obj);
                }
            }

            if (hitRecList.Length > 0)
            {
                _rigidbody.velocity = attackRecoil;
            }

            yield return new WaitForSeconds(effectDelay);

            attackEffect.SetActive(false);

            // attack cooldown
            _isAttackable = false;
            yield return new WaitForSeconds(attackInterval);
            _isAttackable = true;
        }
    }
}
