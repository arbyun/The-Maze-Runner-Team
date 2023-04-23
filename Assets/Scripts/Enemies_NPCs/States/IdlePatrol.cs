using Enemies_NPCs.Enemy_Behaviour;

namespace Enemies_NPCs.States
{
    public class IdlePatrol: PatrolState
    {
        /// <summary> Override checks if the Patroller has reached an edge.</summary>
        /// <param name="patrolController"> The patrol enemy.</param>
        /// <param name="direction"> Optional, used to check if the direction of the patroller is valid.</param>
        /// <returns> A boolean value. it returns true if the reachedge function returns 0, and false otherwise.</returns>
        public override bool CheckValid(Patroller patrolController, string direction = null)
        {
            return patrolController.ReachEdge() == 0;
        }

        public override void Execute(Patroller patrolController, string direction = null)
        {
            patrolController.Walk(0);
        }

    }
}