using UnityEngine;

namespace Dialogue.Interaction
{
    public class DialogInteractor : MonoBehaviour
    {
        [SerializeField] private float detectionRadius;
        [SerializeField] private Transform origin;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private GameObject bubbleSprite;
        [SerializeField] private float timeForReset = 2f;

        [SerializeField] private Models.Interaction interaction;
        private IDialogInteract _dialogInteract;

        private bool _isBubbleShown;
        private float _timeSinceFirstShown;

        private void Awake()
        {
            HideBubble();
        }

        private void Update()
        {
            var hit = Physics2D.CircleCast(origin.position, detectionRadius, Vector2.right, 
                detectionRadius, targetMask);
            if (hit)
            {
                if (!interaction.HasAnyDialogueLeft())
                {
                    if (_isBubbleShown)
                    {
                        _isBubbleShown = false;
                        HideBubble();
                    }

                    return;
                }

                if (hit.transform.TryGetComponent(out IDialogInteract interact))
                {
                    if (!_isBubbleShown)
                    {
                        _isBubbleShown = true;
                        ShowBubble();
                    }

                    _dialogInteract = interact;

                    _timeSinceFirstShown = timeForReset;

                    interact.ReadyForInteraction(interaction);
                }
            }
            else
            {
                if (_dialogInteract != null)
                {
                    _dialogInteract.CancelInteraction();
                    _dialogInteract = null;
                }

                if (_isBubbleShown)
                {
                    _isBubbleShown = false;
                    HideBubble();
                }
            }

            // we subtract the reset timer if the player leaves
            if (_dialogInteract == null)
            {
                _timeSinceFirstShown -= Time.deltaTime;

                if (_timeSinceFirstShown <= 0) interaction.Reset();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(origin.position, detectionRadius);
        }

        private void ShowBubble()
        {
            bubbleSprite.SetActive(true);
        }

        private void HideBubble()
        {
            bubbleSprite.SetActive(false);
        }
    }
}