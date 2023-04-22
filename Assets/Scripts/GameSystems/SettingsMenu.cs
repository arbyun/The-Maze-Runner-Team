using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameSystems
{
    public class SettingsMenu : MonoBehaviour
    {
        public InputActionReference moveRightAction;
        public InputActionReference moveLeftAction;
        public InputActionReference jumpAction;
        public InputActionReference attackAction;
        public InputActionReference escapeAction;
        public InputActionReference openInventoryAction;
        public InputActionReference switchWeaponsAction;
        public InputActionReference dashAction;
        public InputActionReference climbUpAction;
        public InputActionReference climbDownAction;
        
        public Button moveRightButton;
        public Button moveLeftButton;
        public Button jumpButton;
        public Button attackButton;
        public Button escapeButton;
        public Button openInventoryButton;
        public Button switchWeaponsButton;
        public Button dashButton;
        public Button climbUpButton;
        public Button climbDownButton;

        /// <summary> Assigns a function to each button's onClick event, which will start a coroutine that rebinds
        /// the action associated with that button.</summary>
        private void Start()
        {
            moveRightButton.GetComponentInChildren<Text>().text = moveRightAction.action.name;
            moveLeftButton.GetComponentInChildren<Text>().text = moveLeftAction.action.name;
            jumpButton.GetComponentInChildren<Text>().text = jumpAction.action.name;
            attackButton.GetComponentInChildren<Text>().text = attackAction.action.name;
            escapeButton.GetComponentInChildren<Text>().text = escapeAction.action.name;
            openInventoryButton.GetComponentInChildren<Text>().text = openInventoryAction.action.name;
            switchWeaponsButton.GetComponentInChildren<Text>().text = switchWeaponsAction.action.name;
            dashButton.GetComponentInChildren<Text>().text = dashAction.action.name;
            climbUpButton.GetComponentInChildren<Text>().text = climbUpAction.action.name;
            climbDownButton.GetComponentInChildren<Text>().text = climbDownAction.action.name;
            
            moveRightButton.onClick.AddListener(() => StartCoroutine(RebindAction(moveRightAction)));
            moveLeftButton.onClick.AddListener(() => StartCoroutine(RebindAction(moveLeftAction)));
            jumpButton.onClick.AddListener(() => StartCoroutine(RebindAction(jumpAction)));
            attackButton.onClick.AddListener(() => StartCoroutine(RebindAction(attackAction)));
            escapeButton.onClick.AddListener(() => StartCoroutine(RebindAction(escapeAction)));
            openInventoryButton.onClick.AddListener(() => StartCoroutine(RebindAction(openInventoryAction)));
            switchWeaponsButton.onClick.AddListener(() => StartCoroutine(RebindAction(switchWeaponsAction)));
            dashButton.onClick.AddListener(() => StartCoroutine(RebindAction(dashAction)));
            climbUpButton.onClick.AddListener(() => StartCoroutine(RebindAction(climbUpAction)));
            climbDownButton.onClick.AddListener(() => StartCoroutine(RebindAction(climbDownAction)));
        }

        /// <summary> Coroutine that waits for the user to input a new keybinding.        
        /// It then applies the binding override to all controls matching the newly bound control.</summary>
        /// <param name="actionToRebind"> The action that is being rebound</param>
        /// <returns> A ienumerator, which is a type of object that can be used to access all the elements of an
        /// array or collection one by one.</returns>
        private IEnumerator RebindAction(InputAction actionToRebind)
        {
            var waitForInput = actionToRebind.PerformInteractiveRebinding()
                .WithTargetBinding(0)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation =>
                {
                    actionToRebind.ApplyBindingOverridesOnMatchingControls(operation.selectedControl);
                })
                .OnCancel(operation =>
                {
                    Debug.Log("Canceled rebind for " + actionToRebind.name);
                })
                .Start();

            yield return waitForInput;
        }
    }
}