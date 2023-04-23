using System.Runtime.InteropServices;
using Enemies_NPCs.Enemy_Behaviour;

namespace Enemies_NPCs.States
{
    public abstract class PatrolState
    {
        public abstract bool CheckValid(Patroller enemyController, [Optional] string direction);
        public abstract void Execute(Patroller enemyController, [Optional] string direction);
    }
}