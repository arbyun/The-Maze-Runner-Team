using System.Linq;
using UnityEngine;
using Utilities;

namespace GameSystems
{
    public class Speaker : MonoBehaviour
    {
        private Interactable _interactable;

        public string[] dialogueLines;
        private readonly bool[] _requireConditions = new bool[5];
        
        [Header("Require conditions to activate dialogue lines? If yes, which lines? (Max of 5)")]
        [ConditionalHide("_requireConditions[1]", true)] public bool element0;
        [ConditionalHide("_requireConditions[2]", true)] public bool element1;
        [ConditionalHide("_requireConditions[3]", true)] public bool element2;
        [ConditionalHide("_requireConditions[4]", true)] public bool element3;
        [ConditionalHide("_requireConditions[5]", true)] public bool element4;

        [ConditionalHide("element0", true)] public Condition Condition0;
        [ConditionalHide("element1", true)] public Condition Condition1;
        [ConditionalHide("element2", true)] public Condition Condition2;
        [ConditionalHide("element3", true)] public Condition Condition3;
        [ConditionalHide("element4", true)] public Condition Condition4;
        
        private void OnEnable()
        {
            _interactable = new Interactable
            {
                InType = Interactable.InteractionTypes.Speak
            };
        }

        private void OnGUI()
        {
            for (int i = 1; i < dialogueLines.Length; i++)
            {
                _requireConditions[i] = true;
            }
        }

        private void Start()
        {
            foreach (var t in dialogueLines)
            {
                bool found = false;

                foreach (var dialogueArray in DialogueSystem.Dialogues.Values)
                {
                    if (dialogueArray.Any(dialogueData => dialogueData.DialogueLine == t))
                    {
                        found = true;
                    }

                    if (found)
                    {
                        break;
                    }
                }
                
                if (!found)
                {
                    if (_requireConditions[1])
                    {
                        DialogueSystem.PopulateDictionary(t, gameObject, true);
                    }
                    
                    if (_requireConditions[2])
                    {
                        DialogueSystem.PopulateDictionary(t, gameObject, cond2: true);
                    }
                    
                    if (_requireConditions[3])
                    {
                        DialogueSystem.PopulateDictionary(t, gameObject, cond3: true);
                    }
                    
                    if (_requireConditions[4])
                    {
                        DialogueSystem.PopulateDictionary(t, gameObject, cond4: true);
                    }
                    
                    if (_requireConditions[5])
                    {
                        DialogueSystem.PopulateDictionary(t, gameObject, cond5: true);
                    }
                    
                    else
                    {
                        DialogueSystem.PopulateDictionary(t, gameObject);
                    }
                    
                }
            }
        }

        private void Update()
        {
            _interactable.OnSpeak(gameObject);
        }
    }
}