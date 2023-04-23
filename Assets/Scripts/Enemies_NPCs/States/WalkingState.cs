

using Enemies_NPCs.Enemy_Behaviour;

namespace Enemies_NPCs.States
{
    public class WalkingState: PatrolState
    {
        private string _direction;

        /// <summary> The WalkingState function is a constructor that sets the direction of the enemy's
        /// movement.</summary>
        /// <param name="direction"> The direction that the enemy will be facing when walking. </param>
        public WalkingState(string direction)
        {
            _direction = direction;
        }

        /// <summary> Override checks if the direction is valid for the given patrol controller.</summary>
        /// <param name="patrolController"> The patrol controller that is being checked.</param>
        /// <param name="direction"> The direction that the patroller is moving.</param>
        /// <returns> A boolean value that determines whether or not the patroller can move in a certain direction.</returns>
        public override bool CheckValid(Patroller patrolController, string direction)
        {
            if (direction == "left")
            {
                return patrolController.ReachEdge() != -1;
            }
            else
            {
                return patrolController.ReachEdge() != 1;
            }
            
        }

        /// <summary> Override uses parameter to determine which direction the patroller should walk.</summary>
        /// <param name="patrolController"> The patroller </param>
        /// <param name="direction"> The direction that the patroller will walk. </param>
        public override void Execute(Patroller patrolController, string direction)
        {
            if (direction == "left")
            {
                patrolController.Walk(-1);
            }
            else
            {
                patrolController.Walk(1);
            }
        }
    }
}