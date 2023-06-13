using System;
using System.Collections;
using Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Player
{
    public class PlayerController : MonoBehaviour, IDataType
    {
        [Header("Movement")]
        public float moveSpeed;
        public float jumpSpeed;
        public int jumpLeft;
        public Vector2 climbJumpForce;
        public float fallSpeed;
    
        [Header("Dashing")]
        public float sprintSpeed;
        public float sprintTime;
        public float sprintInterval;
        public Vector2 sprintDirection;
    
        [Header("Combat")]
        public float attackInterval;
        public int health;
        public Color invulnerableColor;
        public Vector2 hurtRecoil;
        public float hurtTime;
        public float hurtRecoverTime;
        public Vector2 deathRecoil;
        public float deathDelay;
        internal Action wasHurt;
        internal bool isDead;

        public Vector2 attackUpRecoil;
        public Vector2 attackForwardRecoil;
        public Vector2 attackDownRecoil;

        public GameObject attackUpEffect;
        public GameObject attackForwardEffect;
        public GameObject attackDownEffect;

        [Header("Debug")]
        public KeyCode testDamage = KeyCode.T;

        internal RaycastHit2D[] enemiesHit;

        [Header("Booleans")] internal bool _isGrounded;
        private bool _isClimb;
        internal bool isSprintable;
        internal bool isSprintReset;
        private bool _isInputEnabled;
        private bool _isFalling;
        private bool _isAttackable;

        [Header("Other Components")]
        private float _climbJumpDelay = 0.2f;
        private float _attackEffectLifeTime = 0.05f;

        internal Animator animator;
        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;
        private TrailRenderer _trailRenderer;

        [SerializeField] internal Weapon equippedWeapon;
        [SerializeField] internal Weapon offhandWeapon;
        
        public delegate void MethodToOverrideHandler();
        public event MethodToOverrideHandler MethodToOverrideEvent;

        // Start is called before the first frame update
        private void Start() {
            _isInputEnabled = true;
            isSprintReset = true;
            _isAttackable = true;

            animator = gameObject.GetComponent<Animator>();
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _transform = gameObject.GetComponent<Transform>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _boxCollider = gameObject.GetComponent<BoxCollider2D>();
            _trailRenderer = gameObject.GetComponent<TrailRenderer>();
        }

        // Update is called once per frame
        private void Update()
        {
            updatePlayerState();
            if (_isInputEnabled)
            {
                move();
                jumpControl();
                fallControl();
                sprintControl();
                attackControl();
            }

            if (Input.GetKeyDown(testDamage))
            {
                hurt(1);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // enter climb state
            if (collision.collider.tag == "Wall" && !_isGrounded)
            {
                _rigidbody.gravityScale = 0;

                Vector2 newVelocity;
                newVelocity.x = 0;
                newVelocity.y = -2;

                _rigidbody.velocity = newVelocity;

                _isClimb = true;
                animator.SetBool("IsClimb", true);

                isSprintable = true;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.tag == "Wall" && _isFalling && !_isClimb)
            {
                OnCollisionEnter2D(collision);
            }
        }

        public void hurt(int damage)
        {
            wasHurt?.Invoke();

            gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");

            health = Math.Max(health - damage, 0);

            if (health == 0)
            {
                die();
                return;
            }

            // enter invulnerable state
            animator.SetTrigger("IsHurt");

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

            StartCoroutine(recoverFromHurtCoroutine());
        }

        private IEnumerator recoverFromHurtCoroutine()
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
            if (collision.collider.tag == "Wall")
            {
                _isClimb = false;
                animator.SetBool("IsClimb", false);

                _rigidbody.gravityScale = 1;
            }
        }

        /* ######################################################### */

        private void updatePlayerState()
        {
            _isGrounded = checkGrounded();
            animator.SetBool("IsGround", _isGrounded);

            float verticalVelocity = _rigidbody.velocity.y;
            animator.SetBool("IsDown", verticalVelocity < 0);

            if (_isGrounded && verticalVelocity == 0)
            {
                animator.SetBool("IsJump", false);
                animator.ResetTrigger("IsJumpFirst");
                animator.ResetTrigger("IsJumpSecond");
                animator.SetBool("IsDown", false);

                jumpLeft = 2;
                _isClimb = false;
                isSprintable = true;
            }
            else if(_isClimb)
            {
                // one remaining jump chance after climbing
                jumpLeft = 1;
            }
        }

        private void move()
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
                        animator.SetTrigger("IsRotate");
                    }
                }
                else if (moveDirection > 0)
                {
                    // move forward
                    animator.SetBool("IsRun", true);
                }
            }

            // stop
            if (Input.GetAxis("Horizontal") == 0)
            {
                animator.SetTrigger("stopTrigger");
                animator.ResetTrigger("IsRotate");
                animator.SetBool("IsRun", false);
            }
            else
            {
                animator.ResetTrigger("stopTrigger");
            }
        }

        private void jumpControl()
        {
            if (!Input.GetButtonDown("Jump"))
                return;

            if (_isClimb)
                climbJump();
            else if (jumpLeft > 0)
                jump();
        }

        private void fallControl()
        {
            if (Input.GetButtonUp("Jump") && !_isClimb)
            {
                _isFalling = true;
                fall();
            } else
            {
                _isFalling = false;
            }
        }

        private void sprintControl()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && isSprintable && isSprintReset)
            {
                _trailRenderer.emitting = true;
                sprintDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (sprintDirection == Vector2.zero)
                {
                    sprintDirection = new Vector2(transform.localScale.x, 0);
                }
                sprint();
            }
            
        }

        private void attackControl()
        {
            if (Input.GetKeyDown(KeyCode.Z) && !_isClimb && _isAttackable)
                attack();
        }

        private void die()
        {
            animator.SetTrigger("IsDead");

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

            StartCoroutine(deathCoroutine());
        }

        private IEnumerator deathCoroutine()
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

            isDead = true;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /* ######################################################### */

        private bool checkGrounded()
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

        private void jump()
        {
            Vector2 newVelocity;
            newVelocity.x = _rigidbody.velocity.x;
            newVelocity.y = jumpSpeed;

            _rigidbody.velocity = newVelocity;

            animator.SetBool("IsJump", true);
            jumpLeft -= 1;
            if (jumpLeft == 0)
            {
                animator.SetTrigger("IsJumpSecond");
            } 
            else if (jumpLeft == 1)
            {
                animator.SetTrigger("IsJumpFirst");
            }
        }

        private void climbJump()
        {
            Vector2 realClimbJumpForce;
            realClimbJumpForce.x = climbJumpForce.x * transform.localScale.x;
            realClimbJumpForce.y = climbJumpForce.y;
            _rigidbody.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

            animator.SetTrigger("IsClimbJump");
            animator.SetTrigger("IsJumpFirst");

            _isInputEnabled = false;
            StartCoroutine(climbJumpCoroutine(_climbJumpDelay));
        }

        private IEnumerator climbJumpCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            _isInputEnabled = true;

            animator.ResetTrigger("IsClimbJump");

            // jump to the opposite direction
            Vector3 newScale;
            newScale.x = -transform.localScale.x;
            newScale.y = 1;
            newScale.z = 1;

            transform.localScale = newScale;
        }

        private void fall()
        {
            Vector2 newVelocity;
            newVelocity.x = _rigidbody.velocity.x;
            newVelocity.y = -fallSpeed;

            _rigidbody.velocity = newVelocity;
        }

        private void sprint()
        {
            // reject input during sprinting
            _isInputEnabled = false;
            isSprintable = false;
            isSprintReset = false;

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

            animator.SetTrigger("IsSprint");
            StartCoroutine(sprintCoroutine(sprintTime, sprintInterval));
        }

        private IEnumerator sprintCoroutine(float sprintDelay, float sprintInterval)
        {
            yield return new WaitForSeconds(sprintDelay);
            _trailRenderer.emitting = false;
            _isInputEnabled = true;
            isSprintable = true;

            yield return new WaitForSeconds(sprintInterval);
            isSprintReset = true;
        }

        private void attack()
        {
            MethodToOverrideEvent?.Invoke();
            
            float verticalDirection = Input.GetAxis("Vertical");
            if (verticalDirection > 0)
                attackUp();
            else if (verticalDirection < 0 && !_isGrounded)
                attackDown();
            else
                attackForward();
        }

        private void attackUp()
        {
            animator.SetTrigger("IsAttackUp");
            attackUpEffect.SetActive(true);

            Vector2 detectDirection;
            detectDirection.x = 0;
            detectDirection.y = 1;

            StartCoroutine(attackCoroutine(attackUpEffect, _attackEffectLifeTime, attackInterval, detectDirection, attackUpRecoil));
        }

        private void attackForward()
        {
            animator.SetTrigger("IsAttack");
            attackForwardEffect.SetActive(true);

            Vector2 detectDirection;
            detectDirection.x = -transform.localScale.x;
            detectDirection.y = 0;

            Vector2 recoil;
            recoil.x = transform.localScale.x > 0 ? -attackForwardRecoil.x : attackForwardRecoil.x;
            recoil.y = attackForwardRecoil.y;

            StartCoroutine(attackCoroutine(attackForwardEffect, _attackEffectLifeTime, attackInterval, detectDirection, recoil));
        }

        private void attackDown()
        {
            animator.SetTrigger("IsAttackDown");
            attackDownEffect.SetActive(true);

            Vector2 detectDirection;
            detectDirection.x = 0;
            detectDirection.y = -1;

            StartCoroutine(attackCoroutine(attackDownEffect, _attackEffectLifeTime, attackInterval, detectDirection, attackDownRecoil));
        }

        internal IEnumerator attackCoroutine(GameObject attackEffect,float effectDelay, float attackInterval, Vector2 detectDirection, Vector2 attackRecoil)
        {
            Vector2 origin = _transform.position;

            float radius = 0.6f;

            float distance = 1.5f;
            LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Trap") | LayerMask.GetMask("Switch") | LayerMask.GetMask("Projectile");

            RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);
            enemiesHit = hitRecList;

            foreach (RaycastHit2D hitRec in hitRecList)
            {
                GameObject obj = hitRec.collider.gameObject;

                string layerName = LayerMask.LayerToName(obj.layer);
            
                if (layerName == "Switch")
                {
                    Switch swithComponent = obj.GetComponent<Switch>();
                    if (swithComponent != null)
                        swithComponent.turnOn();
                } 
                else if (layerName == "Enemy")
                {
                    EnemyController enemyController = obj.GetComponent<EnemyController>();
                    if (enemyController != null)
                        enemyController.hurt(1);
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

            // attack cool down
            _isAttackable = false;
            yield return new WaitForSeconds(attackInterval);
            _isAttackable = true;
        }
    }
}
