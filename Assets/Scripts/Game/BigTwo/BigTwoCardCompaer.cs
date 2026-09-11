using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 卡牌配對規則(運算主責腳本)
    /// </summary>
    public class BigTwoCardCompaer : IComparer<PlayingCard>
    {

        #region 公開方法
        /// <summary>
        /// 先比點數再比花色
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public int Compare(PlayingCard left, PlayingCard right)
        {
            //取得左邊的卡相對右邊的大小向性(負：較小，0：相等，正：較大)
            int rankCompaer = GetRankStrength(left.Rank).CompareTo(GetRankStrength(right.Rank));
            //數字不分勝負時，使用花色回傳
            return rankCompaer != 0 ? rankCompaer : left.Suit.CompareTo(right.Suit);
        }
        /// <summary>
        /// 取得點數值強度
        /// 將 A 和 2 偏移置 K 後面，相當於14、15
        /// </summary>
        /// <param name="rank">原本的點數值</param>
        /// <returns>翻譯後的點數值</returns>
        public int GetRankStrength(Rank rank)
        {
            switch (rank)
            {
                case Rank.Ace: return 14;
                case Rank.Two: return 15;
                default: return (int)rank;
            }
        }
        #endregion 公開方法
    }
}

