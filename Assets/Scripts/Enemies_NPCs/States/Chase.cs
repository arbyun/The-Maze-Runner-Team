using System;
using Enemies_NPCs.Enemy_Behaviour;

namespace Enemies_NPCs.States
{
    public class Chase: State
    {
        /// <summary> Override checks if the enemy is within a certain distance of the player.        
        /// If it is, then it returns true.</summary>
        /// <param name="enemyController"> The enemy.</param>
        public override bool CheckValid(Enemy_Behaviour.Enemy enemyController)
        {
            float playerEnemyDistanceAbs = Math.Abs(enemyController.PlayerEnemyDistance());
            return playerEnemyDistanceAbs <= enemyController.detectDistance;
        }

       
        /// <summary> Override calculates the distance between the patroller and the player using the
        /// `PlayerEnemyDistance` method of the `Patroller` object. If the absolute value of the distance is less
        /// than 0.1, it sets the walking speed of the patroller to 0, otherwise it sets the walking speed to the
        /// distance.</summary>
        /// <param name="enemyController">The enemy.</param>
        public override void Execute(Enemy_Behaviour.Enemy enemyController)
        {
            Patroller patrolController = (Patroller)enemyController;
            float dist = patrolController.PlayerEnemyDistance();
            patrolController.Walk(Math.Abs(dist) < 0.1f ? 0 : dist);
        }
    }
}