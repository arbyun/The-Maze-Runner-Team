using GameSystems.Mechanics;
using UnityEngine;
using Utilities;

namespace GameSystems
{
    public class MovementMechanics : MonoBehaviour
    {

        [Header("DEBUG")] [Header("Allow Jump")]
        public bool jumpMode;

        [Header("Allow Dash")] public bool dashMode;
        [Header("Allow Grapple")] public bool grappleMode;
        [Header("Allow Wall Jump")] public bool wallJumpMode;
        [Header("Allow Gravity Pull")] public bool gravityMode;

        [ConditionalHide("gravityMode", true)] public float gravityAmount;
        
        /// <summary> This function will allow all of the abilities that are set to true in the inspector.</summary>
        private void OnEnable()
        {
            if (jumpMode)
            {
                AllowJump();
            }

            if (grappleMode)
            {
                AllowGrapple();
            }

            if (dashMode)
            {
                AllowDash();
            }

            if (wallJumpMode)
            {
                AllowWallJump();
            }

            if (gravityMode)
            {
                AllowGravity();
            }
        }

        private void AllowJump()
        {
            gameObject.AddComponent<Jump>();
        }

        private void AllowDash()
        {
            gameObject.AddComponent<Dash>();
        }

        private void AllowGrapple()
        {
            gameObject.AddComponent<Grapple>();
        }

        private void AllowWallJump()
        {
            gameObject.AddComponent<WallJump>();
        }

        /// <summary> Aallows the player to fall down if they are not on a platform.</summary> 
        private void AllowGravity()
        {
            if (gameObject.TryGetComponent<Rigidbody2D>(out var rb2d) && 
                rb2d.bodyType == RigidbodyType2D.Dynamic)
            {
                if (rb2d.gravityScale <= 0)
                {
                    rb2d.gravityScale = gravityAmount;
                }
                else
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = gravityAmount;
                }
            }
        }

    }
}
