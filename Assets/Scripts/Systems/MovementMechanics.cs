using Systems.Mechanics;
using UnityEngine;
using Utilities;

namespace Systems
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

        private void AllowGravity()
        {
            if (gameObject.TryGetComponent<Rigidbody2D>(out var rb2d))
            {
                if (rb2d.bodyType == RigidbodyType2D.Dynamic)
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
}
