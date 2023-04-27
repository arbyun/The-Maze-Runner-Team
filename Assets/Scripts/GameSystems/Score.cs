using System;
using UnityEngine;

namespace GameSystems
{
    public class Score : MonoBehaviour
    {
        /* Formula: points = maxPoints * (1 - timeTaken/180)
             where maxPoints is the maximum number of points awarded for completing the task within 3 minutes, 
             timeTaken is the time taken to complete the task in seconds, and 180 is the total number of seconds 
             present in 3 minutes.*/
        
        private const int MaxPoints = 10000;
        private readonly int _timeLimit = 180;

        /// <summary> The TimeToScore function takes in a time and totalTime, and returns the score that corresponds to
        /// that time. Used to calculate the score of a player based on how much time they have left.</summary>
        /// <param name="time"> The time that the player has taken to complete the level.</param>
        /// <param name="totalTime"> The total time in seconds that the player has to complete the level.</param>
        /// <returns> The score of the player.</returns>
        public double TimeToScore(float time, int totalTime = 180) => MaxPoints * (1 - (double)time / totalTime);        

        
        /// <summary> The GetScore function takes a double type score and returns an int points.        
        /// The function rounds the score to the nearest integer, then casts it as an int.</summary>
        /// <param name="score"> The score of the player</param>
        /// <returns> An integer.</returns>
        public int GetScore(double score) => (int)Math.Round(score);
    }
}