using UnityEngine;

namespace GameSystems.Mechanics
{
    public class WallJump : MonoBehaviour
    {
        private const float Gravity = 1.0f;
        private const float JumpHeight = 15.0f;
        private CharacterController _controller;
        private GameObject _player;

        private Vector3 _wallSurfNormal;
        private Vector3 _direction, _speed;
        private bool _canWallJump;
        private float _yVelocity;

        private float _movementSpeed;

        /// <summary> Finds the player object and assigns it to a variable, then checks if that object has a
        /// CharacterController component attached to it. If not, we log an error message in the console.</summary>
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player.TryGetComponent<CharacterController>(out var controller))
            {
                _controller = _player.GetComponent<CharacterController>();
            }
            else
            {
                Debug.Log("We might need a character controller!");
            }

            _movementSpeed = Player.Speed;
        }

        /// <summary> Handles the player's movement and jumping.</summary>
        private void Update()
        {
            float horizontalInp = Input.GetAxisRaw("Horizontal");

            if (!_controller.isGrounded)
            {
                _canWallJump = false;
                _direction = new Vector3(horizontalInp, 0, 0);
                _speed = _direction * _movementSpeed;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _yVelocity = JumpHeight;
                }
            }
            else if (_controller.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space) && _canWallJump)
                {
                    _yVelocity = JumpHeight;
                    _speed = _wallSurfNormal * _movementSpeed;
                }

                _yVelocity -= Gravity;
            }

            _speed.y = _yVelocity;
            _controller.Move(_speed * Time.deltaTime);
        }

        /// <summary> Called when the controller hits a collider while performing a wall jump</summary>
        /// <param name="hit"> /// the controllercolliderhit parameter is used to get the normal of the surface
        /// that was hit. This allows us to know which direction we should be jumping off of it in. 
        /// </param>
        /// <returns> The normal of the wall that the player is colliding with.</returns>
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_controller.isGrounded == false && hit.transform.CompareTag($"Wall"))
            {
                //Debug.DrawRay(hit.point, hit.normal, Color.blue);
                _wallSurfNormal = hit.normal;
                _canWallJump = true;
            }
        }
    }
}