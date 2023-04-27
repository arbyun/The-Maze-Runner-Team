using Trap;
using UnityEngine;

namespace Switch
{
    public class Switcher : MonoBehaviour
    {
        public Sprite triggered;
        public GameObject obstacle;
        public GameObject trap;

        private SpriteRenderer _spriteRenderer;
        private Obstacle.Obstacle _obstacle;
        private BasicTrap _trap;

        void Start()
        {
            _trap = trap.GetComponent<BasicTrap>();
            _obstacle = obstacle.GetComponent<Obstacle.Obstacle>();
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        public void TurnOn()
        {
            _spriteRenderer.sprite = triggered;

            _obstacle.SelfDestroy();

            _trap.Trigger();

            gameObject.layer = LayerMask.NameToLayer("Decoration");
        }
    }
}
