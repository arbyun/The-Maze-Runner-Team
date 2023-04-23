using System;
using System.Collections;
using Enemies_NPCs.Enemy_Behaviour;
using UnityEngine;

namespace Enemies_NPCs.States
{
    public class Patrol: State
    {
        private PatrolState _currentState;
        private int _currentStateCase = 0;
        private bool _isFinished;   // ready for next state

        /// <summary> The Patrol function is used to set the current state of the patrolling enemy.</summary>
        /// <returns> A bool value</returns>
        public Patrol()
        {
            _currentState = new IdlePatrol();
            _isFinished = true;
        }

        /// <summary> The CheckValid function checks if the enemy is within a certain distance of the player.        
        /// If it is, then it returns false and moves on to the next state in its list.
        /// If not, then it returns true and stays in this state.</summary>
        /// <param name="enemyController"> The enemy. </param>
        /// <returns> True if the player is outside of its detection range, and false otherwise.</returns>
        public override bool CheckValid(Enemy_Behaviour.Enemy enemyController)
        {
            float playerEnemyDistanceAbs = Math.Abs(enemyController.PlayerEnemyDistance());
            return playerEnemyDistanceAbs > enemyController.detectDistance;
        }

        /// <summary> The Execute function is called by the EnemyController class.        
        /// It checks if the current state is valid, and if not, it randomly chooses a new one.
        /// Then it executes that state.</summary>
        /// <param name="enemyController"> The enemy that will execute this behaviour. </param>
        /// <returns> A coroutine that waits for a certain amount of time before changing the state again</returns>
        public override void Execute(Enemy_Behaviour.Enemy enemyController)
        {
            Patroller patrolController = (Patroller)enemyController;
            if (!_currentState.CheckValid(patrolController) || _isFinished)
            {
                // randomly change current state
                int randomStateCase;
                do
                {
                    randomStateCase = UnityEngine.Random.Range(0, 3);
                } while (randomStateCase == _currentStateCase);

                _currentStateCase = randomStateCase;
                switch (_currentStateCase)
                {
                    case 0:
                        _currentState = new IdlePatrol();
                        break;
                    case 1:
                        _currentState = new WalkingState("left");
                        break;
                    case 2:
                        _currentState = new WalkingState("right");
                        break;
                }

                patrolController.StartCoroutine(ExecuteCoroutine(patrolController.BehaveInterval()));
            }

            _currentState.Execute(patrolController);
        }

        /// <summary> The ExecuteCoroutine function is a coroutine that waits for the specified delay before
        /// setting _isFinished to true.</summary>
        /// <param name="delay"> The delay before the coroutine is executed</param>
        /// <returns> An IEnumerator object.</returns>
        private IEnumerator ExecuteCoroutine(float delay)
        {
            _isFinished = false;
            yield return new WaitForSeconds(delay);
            if (!_isFinished)
                _isFinished = true;
        }
    }
}