using Enemies_NPCs.States;
using Player;
using UnityEngine;

namespace Enemies_NPCs.Enemy_Behaviour
{
    public abstract class Enemy : MonoBehaviour
    {
        public int health;
        public float detectDistance;
        public int damageToPlayer;

        public Vector2 hurtRecoil;
        public float hurtRecoilTime;
        public Vector2 deathForce;
        public float destroyDelay;

        protected State CurrentState;
        protected float _playerEnemyDistance;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool tagName = collision.gameObject.CompareTag("Player");

            if (tagName)
            {
                PlayerController playerController = collision.collider.GetComponent<PlayerController>();
                playerController.Hurt(damageToPlayer);
            }
        }
        
        public float PlayerEnemyDistance()
        {
            return _playerEnemyDistance;
        }

        public abstract float BehaveInterval();

        public abstract void Hurt(int damage);

        protected abstract void Die();
    }
}