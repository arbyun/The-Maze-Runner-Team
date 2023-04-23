using Enemies_NPCs.Enemy_Behaviour;
using Player;
using UnityEngine;

namespace Trap
{
    public class Deadly : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

            if (layerName == "Player")
            {
                PlayerController playerController = collision.collider.GetComponent<PlayerController>();
                playerController.Hurt(playerController.health);
            }
            else if (layerName == "Enemy")
            {
                Enemy enemyController = collision.collider.GetComponent<Enemy>();
                enemyController.Hurt(enemyController.health);
            }
        }
    }
}
