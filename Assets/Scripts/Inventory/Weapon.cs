using System;
using System.Collections;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class Weapon: InventoryItem
    {
        public int totalUses;
        public float damageMultiplier;
        public Consume consumeUsesOn;
        public GameObject projectile;
        public bool canShoot;
        public float shootInterval;

        private PlayerController _owner;

        internal void OnEnable()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = itemData.itemInGameImage;
            itemData.itemGameObject = gameObject;

            //inventory.slot.Add(itemData.name, itemData.itemInventoryImage);
            //inventory.itemList.Add(gameObject)

            var combatMode = itemData.types.Contains(ItemType.Ranged) ? "ranged" : "melee";

            if (combatMode != "ranged")
            {
                projectile = null;
            }
            else
            {
                _owner.MethodToOverrideEvent += OnAttackOverride;
            }
        }
        
        private void OnDisable()
        {
            // Unsubscribe from the event when the script is disabled
            _owner.MethodToOverrideEvent -= OnAttackOverride;
        }

        private void OnAttackOverride()
        {
            if (canShoot)
            {
                _owner.animator.SetTrigger("attack");

                canShoot = false;
                Vector2 detectDirection;
                
                float verticalDirection = Input.GetAxis("Vertical");
                if (verticalDirection > 0)
                {
                    detectDirection.x = 0;
                    detectDirection.y = 1;
                }
                else if (verticalDirection < 0 && !_owner._isGrounded)
                {
                    detectDirection.x = 0;
                    detectDirection.y = -1;
                }
                else
                {
                    detectDirection.x = -transform.localScale.x;
                    detectDirection.y = 0;
                }

                StartCoroutine(shootPlayerCoroutine(detectDirection, shootInterval));
            }
        }

        private IEnumerator shootPlayerCoroutine(Vector2 direction, float shootInterval)
        {
            yield return new WaitForSeconds(0.2f);

            var _transform = _owner.gameObject.transform;
                
            // set diretion of shooting
            Vector3 position = _transform.position;
            Quaternion rotation = _transform.rotation;
            GameObject projectileObj = Instantiate(this.projectile, position, rotation);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.direction = direction;
            // shoot the player with projectile
            projectile.trigger();

            yield return new WaitForSeconds(shootInterval);
            if (!canShoot)
                canShoot = true;
        }
        
        private void Start()
        {
            _owner = gameObject.GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            switch (consumeUsesOn)
            {
                case (Consume.OnHit):
                    _owner.wasHurt += hurtEvent;
                    break;
                
                case (Consume.OnAttack):
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        totalUses--;
                    }
                    break;
                
                case (Consume.OnAttackOnlyIfItHits):
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                       if (_owner.enemiesHit.Length > 0)
                       {
                           foreach (RaycastHit2D hitRec in _owner.enemiesHit)
                           {
                               totalUses--;
                           }
                       } 
                    }
                    
                    break;
                
                default:
                    break;
            }
        }

        private void hurtEvent()
        {
            totalUses--;
        }
    }

    public enum Consume
    {
        OnHit,
        OnAttack,
        OnAttackOnlyIfItHits,
    }
}