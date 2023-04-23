namespace Utilities.Interaction
{
    public interface IDialogInteract
    {
        void ReadyForInteraction(Models.Interaction newInteraction);

        void CancelInteraction();

        void Interact();
    }
}