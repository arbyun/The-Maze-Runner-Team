using System.Collections.Generic;
using GameData;
using GameSystems.Inventory;
using UnityEngine;

namespace GameSystems
{
    public class LevelRewards : MonoBehaviour
    {
        public Score playerScore;
        public Items itemDataLoader;
        
        
        /// <summary> The AwardLoot function creates a list of items called loot, then it sets an int score to the value
        /// returned by playerScore.GetScore(time).
        /// If score is greater than or equal to 90, it sets loot equal to itemDataLoader.GetItemsByQuality(3f) filtered
        /// by items with quality less than 4; otherwise if score is greater than or equal to 60, it sets loot equal to
        /// itemDataLoader.GetItemsByQuality(2f) filtered by items with quality between 2 and 4 (inclusive); otherwise
        /// it sets loot to the lowest quality.</summary>
        /// <param name="time"> Amount of time it took to complete the level.</param>
        /// <returns> A list of items that are to be awarded to the player</returns>
        void AwardLoot(float time)
        {
            List<Item> loot;
            int score = playerScore.GetScore(time);

            if (score >= 0.9 * 10000)
            {
                loot = itemDataLoader.GetItemsByQuality(3f).FindAll(item => item.quality < 4);

            }
            else if (score >= 0.6 * 10000)
            {
                loot = itemDataLoader.GetItemsByQuality(2f).FindAll(item => item.quality >= 2 && item.quality < 4);
            }
            else
            {
                loot = itemDataLoader.GetItemsByQuality(2f).FindAll(item => item.quality >= 2);
            }

            ShuffleList(loot);

            loot = loot.GetRange(0, Mathf.Min(3, loot.Count));

            foreach (Item item in loot)
            {
                // award item to the player here
            }
        }

        /// <summary> The ShuffleList function takes a list of type T and shuffles it.</summary>
        /// <param name="list"> The list that is to be shuffled.</param>
        /// <returns> A shuffled list.</returns>
        void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}