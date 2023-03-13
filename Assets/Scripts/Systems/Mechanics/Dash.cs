using System.Collections;
using UnityEngine;

namespace Systems.Mechanics
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
            var playerScript = _player.GetComponent<Player>();
            _speed = playerScript.speed;
        }

        private void Update()
        {
            float horizontalIn = Input.GetAxis("Horizontal");
            float verticalIn = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalIn, verticalIn, 0);
            
            _player.transform.Translate(direction * (_speed * Time.deltaTime));

            _time -= Time.deltaTime;
            
            if (Input.GetKeyDown(_dashKey) && _time <= 0)
            {
                _speed *= 1.5f;
                _time = 2f;
                _hasDashed = true;
            }
            else if (_time > 0 && _hasDashed)
            {
                StartCoroutine(AfterDash());
            }
        }

        private IEnumerator AfterDash()
        {
            yield return new WaitForSeconds(1.2f);
            _speed /= 1.5f;
        }
    }
}