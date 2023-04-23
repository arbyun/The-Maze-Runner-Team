using System;
using Enemies_NPCs.Enemy_Behaviour;

namespace Enemies_NPCs.States
{
    public class Idle: State
    {
        public override bool CheckValid(Enemy enemyController)
        {
            float playerEnemyDistanceAbs = Math.Abs(enemyController.PlayerEnemyDistance());
            return playerEnemyDistanceAbs > enemyController.detectDistance;
        }

        public override void Execute(Enemy enemyController)
        {
            // don't do anything while being idle
        }
    }
}