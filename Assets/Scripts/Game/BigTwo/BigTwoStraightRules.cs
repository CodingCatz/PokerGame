using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 順子的規則靜態類
    /// </summary>
    public static class BigTwoStraightRules
    {
        #region 公開方法
        /// <summary>
        /// 辨識五張不同點是否成順
        /// </summary>
        /// <param name="cards">校驗牌組對象</param>
        /// <param name="strength">回傳強度</param>
        /// <returns>是否成順</returns>
        public static bool TryGetStraight(IReadOnlyList<PlayingCard> cards, out int strength)
        {
            strength = 0;
            if (cards.Count != 5) return false;//基本條件安全回檔機制

            List<int> ranks = new List<int>();//為了產生順子新次序建立清單
            foreach (PlayingCard card in cards) 
            {//以A後面可接2的情況去跑：只把A當14使用，2維持原樣
                int rank = card.Rank == Rank.Ace ? 14 : (int)card.Rank;
                ranks.Add(rank);
            }
            ranks.Sort();
            if (Matches(ranks, 2, 3, 4, 5, 6))
            {//特規最大
                strength = 16;
                return true;
            }
            if (Matches(ranks, 2, 11, 12, 13, 14))
            {//A可接2
                strength = 16;
                return true;
            }
            if (Matches(ranks, 2, 3, 4, 5, 14))
            {//特規次大
                strength = 15;
                return true;
            }

            for (int i = 1; i < ranks.Count; i++)
            {//一般情況：有沒連續(不是前一個的+1號)
                if (ranks[i] != ranks[i - 1] + 1) return false;
            }
            strength = ranks[4];
            return true;
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 特殊規定的符合驗證
        /// </summary>
        private static bool Matches(IReadOnlyList<int> ranks, int a, int b, int c, int d, int e)
        {
            return ranks[0] == a && ranks[1] == b && ranks[2] == c && ranks[3] == d && ranks[4] == e;
        }
        #endregion 私有方法
    }
}

