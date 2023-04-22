using System.Collections;
using UnityEngine;

namespace GameSystems.Mechanics
{
    public class Dash : MonoBehaviour
    {
        private GameObject _player;
        private float _speed;
        private KeyCode _dashKey;
        private float _time = 2f;
        private bool _hasDashed;

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _speed = Player.Speed;
        }

        /// <summary> Checks for input, and if the player has pressed the dash key, it will increase their speed
        /// by 50% for 2 seconds. After that time window has passed, it will start a coroutine to decrease their
        /// speed back to normal.</summary>
        private void Update()
        {
            float horizontalIn = Input.GetAxis("Horizontal");
            float verticalIn = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalIn, verticalIn, 0);
            
            // The actual dashing
            _player.transform.Translate(direction * (_speed * Time.deltaTime));

            // Timer here is not for cd, rather it is for the time window where the player's speed will be increased
            _time -= Time.deltaTime;
            
            if (Input.GetKeyDown(_dashKey) && _time <= 0)
            {
                _speed *= 1.5f;
                _time = 2f;
                _hasDashed = true;
            }
            else if (_time > 0 && _hasDashed)
            {
                // Now we start the actual cooldown
                StartCoroutine(AfterDash());
            }
        }

        /// <summary> Coroutine that returns the player's speed to normal after 1.2 seconds.</summary>
        /// <returns> A float</returns>
        private IEnumerator AfterDash()
        {
            // Let's return to the og speed
            _speed /= 1.5f;
            yield return new WaitForSeconds(1.2f);
        }
    }
}