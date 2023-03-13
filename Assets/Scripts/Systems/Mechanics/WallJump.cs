﻿using System;
using UnityEngine;

namespace Systems.Mechanics
{
    public class WallJump : MonoBehaviour
    {
        private float _gravity = 1.0f;
        private float _jumpHeight = 15.0f;
        private CharacterController _controller;
        private GameObject _player;

        private Vector3 _wallSurfNormal;
        private Vector3 _direction, _speed;
        private bool _canWallJump;
        private float _yVelocity;

        private float _movementSpeed;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player.TryGetComponent<CharacterController>(out var controller))
            {
                _controller = _player.GetComponent<CharacterController>();
            }
            else
            {
                Debug.Log("We need a character controller!");
            }

            var playerScript = _player.GetComponent<Player>();
            _movementSpeed = playerScript.speed;
        }

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
                    _yVelocity = _jumpHeight;
                }
            }
            else if (_controller.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space) && _canWallJump)
                {
                    _yVelocity = _jumpHeight;
                    _speed = _wallSurfNormal * _movementSpeed;
                }

                _yVelocity -= _gravity;
            }

            _speed.y = _yVelocity;
            _controller.Move(_speed * Time.deltaTime);
        }

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