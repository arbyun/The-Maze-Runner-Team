using System;
using System.Collections;
using Enemies_NPCs.States;
using Player;
using UnityEngine;

namespace Enemies_NPCs.Enemy_Behaviour
{
    public class Patroller : Enemy
    {
        public float walkSpeed;
        public float edgeSafeDistance;
        public float behaveIntervalLeast;
        public float behaveIntervalMost;

        private int _reachEdge;
        private bool _isChasing;
        private bool _isMovable;

        private Transform _playerTransform;
        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        void Start()
        {
            _playerTransform = GlobalController.Instance.player.GetComponent<Transform>();
            _transform = gameObject.GetComponent<Transform>();
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _animator = gameObject.GetComponent<Animator>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            CurrentState = new Patrol();

            _isChasing = false;
            _isMovable = true;
        }

        // Update is called once per frame
        void Update()
        {
            // update distance between player and enemy
            _playerEnemyDistance = _playerTransform.position.x - _transform.position.x;

            // update edge detection
            Vector2 detectOffset;
            detectOffset.x = edgeSafeDistance * _transform.localScale.x;
            detectOffset.y = 0;
            _reachEdge = CheckGrounded(detectOffset) ? 0 : (_transform.localScale.x > 0 ? 1 : -1);

            // update state
            if (!CurrentState.CheckValid(this))
            {
                if (_isChasing)
                {
                    CurrentState = new Patrol();
                }
                else
                {
                    CurrentState = new Chase();
                }

                _isChasing = !_isChasing;
            }

            if (_isMovable)
                CurrentState.Execute(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

            if (layerName == "Player")
            {
                PlayerController playerController = collision.collider.GetComponent<PlayerController>();
                playerController.Hurt(1);
            }
        }

        public override float BehaveInterval()
        {
            return UnityEngine.Random.Range(behaveIntervalLeast, behaveIntervalMost);
        }

        public int ReachEdge()
        {
            return _reachEdge;
        }

        public override void Hurt(int damage)
        {
            health = Math.Max(health - damage, 0);

            _isMovable = false;

            if (health == 0)
            {
                Die();
                return;
            }

            Vector2 newVelocity = hurtRecoil;
            newVelocity.x *= _transform.localScale.x;

            _rigidbody.velocity = newVelocity;

            StartCoroutine(HurtCoroutine());
        }

        private IEnumerator HurtCoroutine()
        {
            yield return new WaitForSeconds(hurtRecoilTime);
            _isMovable = true;
        }

        private bool CheckGrounded(Vector2 offset)
        {
            Vector2 origin = _transform.position;
            origin += offset;

            float radius = 0.3f;

            // detect downwards
            Vector2 direction;
            direction.x = 0;
            direction.y = -1;

            float distance = 1.1f;
            LayerMask layerMask = LayerMask.GetMask("Platform");

            RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
            return hitRec.collider != null;
        }

        public void Walk(float move)
        {
            int direction = move > 0 ? 1 : move < 0 ? -1 : 0;

            float newWalkSpeed = (direction == _reachEdge) ? 0 : direction * walkSpeed;

            // flip sprite
            if (direction != 0 && health > 0)
            {
                Vector3 newScale = _transform.localScale;
                newScale.x = direction;
                _transform.localScale = newScale;
            }

            // set velocity
            Vector2 newVelocity = _rigidbody.velocity;
            newVelocity.x = newWalkSpeed;
            _rigidbody.velocity = newVelocity;

            // animation
            _animator.SetFloat("Speed", Math.Abs(newWalkSpeed));
        }

        protected override void Die()
        {
            _animator.SetTrigger("isDead");

            Vector2 newVelocity;
            newVelocity.x = 0;
            newVelocity.y = 0;
            _rigidbody.velocity = newVelocity;

            gameObject.layer = LayerMask.NameToLayer("Decoration");

            Vector2 newForce;
            newForce.x = _transform.localScale.x * deathForce.x;
            newForce.y = deathForce.y;
            _rigidbody.AddForce(newForce, ForceMode2D.Impulse);

            StartCoroutine(FadeCoroutine());
        }

        private IEnumerator FadeCoroutine()
        {

            while (destroyDelay > 0)
            {
                destroyDelay -= Time.deltaTime;

                if (_spriteRenderer.color.a > 0)
                {
                    Color newColor = _spriteRenderer.color;
                    newColor.a -= Time.deltaTime / destroyDelay;
                    _spriteRenderer.color = newColor;
                    yield return null;
                }
            }

            Destroy(gameObject);
        }
    }
}