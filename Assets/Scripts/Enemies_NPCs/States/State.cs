
namespace Enemies_NPCs.States
{
    public abstract class State
    {
        public abstract bool CheckValid(Enemy_Behaviour.Enemy enemyController);
        public abstract void Execute(Enemy_Behaviour.Enemy  enemyController);
    }
}