/*using UnityEngine;

namespace GameSystems
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

        /// <summary> Set the tag of a GameObject based on its Interaction Type.</summary
        /// <param name="gameObject"> /// the game object to be tagged.
        /// </param>
        /// <returns> The gameobject.tag</returns>
        private void GetEnable(GameObject gameObject)
        {
            gameObject.tag = InType switch
            {
                (InteractionTypes.Push) => "Pushable",
                _ => gameObject.tag
            };
        }

        /// <summary> Finds the player object in the scene and assigns it to a variable.</summary>
        /// <returns> The player's position.</returns>
        private void GetStart()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary> Called when the player presses the interact key while in range of an NPC.        
        /// The function checks if the player is within a certain distance from an NPC, and if so, it calls on
        /// DialogueSystem's GetDialogueString function.</summary>
        /// <param name="gameObject"> The gameobject that is being interacted with.</param>
        /// <returns> The string of the dialogue that is assigned to the gameobject</returns>
        internal void OnSpeak(GameObject gameObject)
        {
            Vector3 distance = _player.transform.position - gameObject.transform.position;
            float i = float.Parse(distance.ToString());

            if (i <= 2f && Input.GetKeyDown(_interactKey))
            {
                //DialogueSystem.GetDialogueString(gameObject);
            }
        }

        /// <summary> Collision checker.</summary>
        /// <param name="other"> The other collider involved in this collision.</param>
        /// <returns> A bool.</returns>
        private bool OnStep(Collider other)
        {
            return other == _player.GetComponent<Collider>();
        }

        /// <summary> Checks if the bullet collider has a Weapon component attached to it.</summary>
        /// <param name="bullet"> /// </param>
        /// <returns> A bool value.</returns>
        private static bool OnHit(Collider bullet)
        {
            return bullet.TryGetComponent<Weapon>(out Weapon weapon);
        }
    }
}*/