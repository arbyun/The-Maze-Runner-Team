using Dialogue.Models;
using UnityEngine;

namespace Dialogue
{
    public abstract class DialogueRenderer : MonoBehaviour
    {
        public abstract void Show();

        public abstract void Render(DialogueData dialogue);

        public abstract void Hide();
    }
}