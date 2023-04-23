using UnityEngine;

namespace Obstacle
{
    public class Obstacle : MonoBehaviour
    {
        public void SelfDestroy()
        {
            Destroy(gameObject);
        }

        [SerializeField] private bool pushable; 

        private bool GetPushable() => pushable;

        internal bool SetPushable(bool boolean) => pushable = boolean;
    }
}
