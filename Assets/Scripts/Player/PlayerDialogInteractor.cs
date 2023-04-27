using Dialogue;
using Dialogue.Interaction;
using Dialogue.Models;
using UnityEngine;

namespace Player
{
    public class PlayerDialogInteractor : MonoBehaviour, IDialogInteract
    {
        private Interaction _interaction;

        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (_interaction != null && _playerInput.Interacting)
            {
                Interact();
            }
        }

        public void ReadyForInteraction(Interaction newInteraction)
        {
            _interaction = newInteraction;
        }

        public void CancelInteraction()
        {
            _interaction = null;
            DialogueManager.Instance.HideDialog();
        }

        public void Interact()
        {
            if (_interaction != null)
            {
                DialogueManager.Instance.BeginConversation(_interaction);
            }
        }
    }
}