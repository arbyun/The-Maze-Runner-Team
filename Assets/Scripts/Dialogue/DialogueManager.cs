using System;
using UnityEngine;

namespace Dialogue
{
    public class DialogueManager: MonoBehaviour
    {
        [SerializeField] private DialogueRenderer dialogRenderer;

        private bool _isInConversation;
        private Models.Interaction _lastInteraction;
        public Action OnDialogCancelled = delegate { };

        public Action OnDialogEnds = delegate { };

        /// <summary>
        ///     Subscribe to this actions to listen and act according to this different events
        ///     For example on another script you can do:
        ///     private void Start()
        ///     {
        ///     DialogManager.Instance.OnDialogStart += HandleDialogStart;
        ///     DialogManager.Instance.OnDialogEnds += HandleDialogEnd;
        ///     DialogManager.Instance.OnDialogCancelled += HandleDialogEnd;
        ///     }
        ///     private void OnDisable()
        ///     {
        ///     DialogManager.Instance.OnDialogStart -= HandleDialogStart;
        ///     DialogManager.Instance.OnDialogEnds -= HandleDialogEnd;
        ///     DialogManager.Instance.OnDialogCancelled -= HandleDialogEnd;
        ///     }
        ///     private void HandleDialogStart()
        ///     {
        ///     // Not allow player to move
        ///     }
        ///     private void HandleDialogEnd()
        ///     {
        ///     // Enable player movement
        ///     }
        /// </summary>
        public Action OnDialogStart = delegate { };

        public static DialogueManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        /// <summary> The BeginConversation function is called when the player interacts with an NPC.        
        /// It sets up the conversation by setting _isInConversation to true, and then calls ShowDialog.</summary>
        ///
        /// <param name="Models.Interaction interaction"> /// the interaction to be used for the conversation.
        /// </param>
        ///
        /// <returns> A boolean value.</returns>
        public void BeginConversation(Models.Interaction interaction)
        {
            if (dialogRenderer != null)
                dialogRenderer.Show();

            if (!_isInConversation)
            {
                _isInConversation = true;
                OnDialogStart?.Invoke();
            }

            _lastInteraction = interaction;

            ShowDialog();
        }

        /// <summary> The HideDialog function hides the dialog box and resets the last interaction to null.        
        /// It also invokes either OnDialogCancelled or OnDialogEnds depending on whether there are any dialog left.</summary>
        ///
        ///
        /// <returns> Void.</returns>
        internal void HideDialog()
        {
            if (dialogRenderer != null)
                dialogRenderer.Hide();

            if (_lastInteraction != null)
            {
                if (_lastInteraction.HasAnyDialogueLeft())
                    OnDialogCancelled?.Invoke();
                else
                    OnDialogEnds?.Invoke();

                _lastInteraction = null;
            }

            _isInConversation = false;
        }

        /// <summary> The ShowDialog function is called when the player interacts with an NPC.        
        /// It checks to see if there is a current dialog, and if so, it renders that dialog.</summary>
        ///
        ///
        /// <returns> A bool</returns>
        private void ShowDialog()
        {
            var dialog = _lastInteraction.GetCurrentDialogue();

            if (dialog == null)
            {
                HideDialog();
                return;
            }

            if (dialogRenderer != null)
                dialogRenderer.Render(dialog);
        }

        /// <summary> The MakeChoice function is called when the player makes a choice in a dialog.        
        /// It takes two parameters: the GUID of the dialog that contains this choice, and 
        /// the text of this choice.</summary>
        /// 
        /// <param name="string dialogGuid"> The dialog guid.</param>
        /// <param name="string choice"> The choice made by the player</param>
        /// <param name="dialogGuid"></param>
        /// <param name="choice"></param>
        /// <returns> The dialog that the player chose.</returns>
        public void MakeChoice(string dialogGuid, string choice)
        {
            if (_lastInteraction == null) return;

            var dialog = _lastInteraction.GetCurrentDialogueFromChoice(dialogGuid, choice);

            if (dialog == null)
            {
                HideDialog();
                return;
            }

            if (dialogRenderer != null)
                dialogRenderer.Render(dialog);
        }
    }
}