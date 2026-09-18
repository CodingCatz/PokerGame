using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 存放已識別的出牌型
    /// </summary>
    public class BigTwoPlay
    {
        #region 公開屬性
        /// <summary>
        /// 取得牌型
        /// </summary>
        public BigTwoCombinationType Type { get; }
        /// <summary>
        /// 取得存放卡牌資料清單
        /// </summary>
        public IReadOnlyList<PlayingCard> Cards { get; }
        /// <summary>
        /// 配對組合的強度
        /// </summary>
        public int Strength { get; }
        /// <summary>
        /// 取得牌組卡牌數量
        /// </summary>
        public int Count => Cards.Count;
        #endregion 公開屬性

        #region 建構式
        /// <summary>
        /// 出牌的資料建立
        /// </summary>
        public BigTwoPlay(BigTwoCombinationType type, IReadOnlyList<PlayingCard> cards, int strength)
        {
            Type = type;
            Cards = cards;
            Strength = strength;
        }
        #endregion 建構式
    }
}
