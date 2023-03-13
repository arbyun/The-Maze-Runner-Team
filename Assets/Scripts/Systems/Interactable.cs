using UnityEngine;

namespace Systems
{
    public class Interactable
    {
        public enum InteractionTypes
        {
            Push,
            Hit,
            Speak,
            Step
        }

        public InteractionTypes InType;
        private KeyCode _interactKey;
        private GameObject _player;

        private void GetEnable(GameObject gameObject)
        {
            gameObject.tag = InType switch
            {
                (InteractionTypes.Push) => "Pushable",
                _ => gameObject.tag
            };
        }

        private void GetStart()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        internal void OnSpeak(GameObject gameObject)
        {
            Vector3 distance = _player.transform.position - gameObject.transform.position;
            float i = float.Parse(distance.ToString());

            if (i <= 2f)
            {
                if (Input.GetKeyDown(_interactKey))
                {
                    DialogueSystem.GetDialogueString(gameObject);
                }
            }
        }

        private bool OnStep(Collider other)
        {
            return other == _player.GetComponent<Collider>();
        }

        private static bool OnHit(Collider bullet)
        {
            return bullet.TryGetComponent<Weapon>(out Weapon weapon);
        }
    }
}