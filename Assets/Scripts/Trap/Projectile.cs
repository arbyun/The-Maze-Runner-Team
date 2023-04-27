using System.Collections;
using Enemies_NPCs.Enemy_Behaviour;
using Player;
using UnityEngine;

namespace Trap
{
    public class Projectile : BasicTrap
    {
        public Vector2 direction;
        public int projectileDamage;
        public float movingSpeed;
        public float destroyTime;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

            if (layerName == "Player")
            {
                PlayerController playerController = collision.collider.GetComponent<PlayerController>();
                playerController.Hurt(projectileDamage);
            }
            
            if (layerName == "Enemy")
            {
                Enemy enemyController = collision.collider.GetComponent<Enemy>();
                enemyController.Hurt(projectileDamage);
            }
        }

        public override void Trigger()
        {
            Vector2 newVelocity = direction.normalized * movingSpeed;
            gameObject.GetComponent<Rigidbody2D>().velocity = newVelocity;

            StartCoroutine(DestroyCoroutine(destroyTime));
        }

        private IEnumerator DestroyCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(this);
        }
    }
}
