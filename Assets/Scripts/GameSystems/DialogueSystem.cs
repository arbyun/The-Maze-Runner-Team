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

        public static string GetDialogueString(GameObject gj, bool cond1 = false, bool cond2 = false, bool cond3 = false, 
            bool cond4 = false, bool cond5 = false)
        {
            if (Dialogues.ContainsKey(gj))
            {
                for (int i = 0; i < Dialogues[gj].Length; i++)
                {
                    if ((cond1 && Dialogues[gj][i].Cond1) | (cond2 && Dialogues[gj][i].Cond2) | 
                        (cond3 && Dialogues[gj][i].Cond3) | (cond4 && Dialogues[gj][i].Cond4) | 
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