using System.Collections.Generic;
using UnityEngine;

namespace GameSystems
{
    public class DialogueData
    {
        public readonly string DialogueLine;
        public readonly bool Cond1;
        public readonly bool Cond2;
        public readonly bool Cond3;
        public readonly bool Cond4;
        public readonly bool Cond5;

        /// <summary> The DialogueData function is a constructor for the DialogueData class.        
        /// It takes in a string and five boolean values, which are used to determine 
        /// whether or not the dialogue line should be displayed.</summary>
        ///
        /// <param name="string DialogueLine"> /// the text that will be displayed in the dialogue box.
        /// </param>
        /// <param name="bool Cond1"> /// the first condition to check. if this is true, the dialogueline will be displayed.
        /// </param>
        /// <param name="bool Cond2"> /// this is the second condition that must be met to display this line of dialogue.
        /// </param>
        /// <param name="bool Cond3"> /// this is a parameter that can be used to check if the dialogue line should be displayed or not. 
        /// if this condition is true, then the dialogue line will display. 
        /// </param>
        /// <param name="bool Cond4"> /// this is a parameter that can be used to check if the player has done something.
        /// </param>
        /// <param name="bool Cond5"> /// the fifth condition that must be met for the dialogue to be displayed.
        /// </param>
        ///
        /// <returns> The dialogue line, and the conditions.</returns>
        public DialogueData(string DialogueLine, bool Cond1 = false, bool Cond2 = false, 
            bool Cond3 = false, bool Cond4 = false, bool Cond5 = false)
        {
            this.DialogueLine = DialogueLine;
            this.Cond1 = Cond1;
            this.Cond2 = Cond2;
            this.Cond3 = Cond3;
            this.Cond4 = Cond4;
            this.Cond5 = Cond5;
        }
    }
    
    public class DialogueSystem : MonoBehaviour
    {
        public static Dictionary<GameObject, DialogueData[]> Dialogues;

        private void OnEnable()
        {
            Dialogues = new Dictionary<GameObject, DialogueData[]>();
        }

        /// <summary> Populates the Dialogues dictionary with dialogue data.        
        /// The function takes in a string, a GameObject, and five boolean values as parameters. 
        /// If any of the booleans are true, then that means that there is an additional condition for
        /// this dialogue to be displayed. 
        /// For example: if cond2 = true, then this means that the player must have completed condition number 2
        /// before they can see this dialogue.</summary>
        /// <param name="dialogueLine"> The dialogue line to be added.</param>
        /// <param name="speaker"> /// the game object that is speaking the dialogue.
        /// </param>
        /// <param name="cond1"> ///etc. Optional conditions (up to 5) to determine whether or not the dialogue
        /// should be displayed. If true, the dialogue will be added to the dictionary with a condition of 1.
        /// </param>
        /// <returns> A dictionary of gameobjects and dialoguedata[]</returns>
        public static void PopulateDictionary(string dialogueLine, GameObject speaker, bool cond1 = false, bool cond2 
            = false, bool cond3 = false, bool cond4 = false, bool cond5 = false)
        {
            if (cond1)
            {
                DialogueData[] dialogue = new DialogueData[] 
                {
                    new DialogueData(dialogueLine, true)
                };
                
                Dialogues.Add(speaker, dialogue);
            }

            if (cond2)
            {
                DialogueData[] dialogue = new DialogueData[] 
                {
                    new DialogueData(dialogueLine, Cond2: true)
                };
                
                Dialogues.Add(speaker, dialogue);
            }
            
            if (cond3)
            {
                DialogueData[] dialogue = new DialogueData[] 
                {
                    new DialogueData(dialogueLine, Cond3: true)
                };
                
                Dialogues.Add(speaker, dialogue);
            }
            
            if (cond4)
            {
                DialogueData[] dialogue = new DialogueData[] 
                {
                    new DialogueData(dialogueLine, Cond4: true)
                };
                
                Dialogues.Add(speaker, dialogue);
            }
            
            if (cond5)
            {
                DialogueData[] dialogue = new DialogueData[] 
                {
                    new DialogueData(dialogueLine, Cond5: true)
                };
                
                Dialogues.Add(speaker, dialogue);
            }

            else
            {
                DialogueData[] dialogue = new DialogueData[] 
                {
                    new DialogueData(dialogueLine)
                };
                
                Dialogues.Add(speaker, dialogue);
            }
        }

        /// <summary> Takes in a GameObject and up to five boolean conditions.        
        /// It then searches through the Dialogues dictionary for the given GameObject, and returns 
        /// the first dialogue line that matches all of those conditions.</summary>
        /// <param name="gj"> The gameobject that is speaking</param>
        /// <returns> A string that is the dialogue line of a specific gameobject.</returns>
        public static string GetDialogueString(GameObject gj, bool cond1 = false, bool cond2 = false, bool cond3 = false, 
            bool cond4 = false, bool cond5 = false)
        {
            if (Dialogues.ContainsKey(gj))
            {
                for (int i = 0; i < Dialogues[gj].Length; i++)
                {
                    if ((cond1 && Dialogues[gj][i].Cond1) || (cond2 && Dialogues[gj][i].Cond2) || 
                        (cond3 && Dialogues[gj][i].Cond3) || (cond4 && Dialogues[gj][i].Cond4) || 
                        (cond5 && Dialogues[gj][i].Cond5))
                    {
                        return Dialogues[gj][i].DialogueLine;
                    }
                }
            }

            return null;
        }
    }
}