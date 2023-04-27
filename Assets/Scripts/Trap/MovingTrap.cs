using System;
using UnityEngine;

namespace Trap
{
    public class MovingTrap : BasicTrap
    {
        public float movingSpeed;
        public float movingLimit;
        public float movingOffset;

        private Vector3 _basePosition;

        private Transform _transform;

        // Start is called before the first frame update
        void Start()
        {
            _transform = gameObject.GetComponent<Transform>();
            _basePosition = _transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            float newOffset = movingOffset + Time.deltaTime * movingSpeed;
            if (Math.Abs(newOffset) >= movingLimit)
            {
                movingSpeed = -movingSpeed;
                _basePosition.x += movingOffset;
                movingOffset = 0;
            }
            else
            {
                movingOffset = newOffset;
            }

            Vector3 newPosition = _basePosition;
            newPosition.x += movingOffset;
            _transform.position = newPosition;
        }

        public override void Trigger()
        {
        
        }
    }
}
