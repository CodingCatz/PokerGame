using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 列出目前能辨識的大老二排型種類
    /// </summary>
    public enum BigTwoCombinationType
    {
        /// <summary>
        /// 不符合任何牌型組合
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 一張牌
        /// </summary>
        Single = 1,
        /// <summary>
        /// 兩張同點
        /// </summary>
        Pair = 2,
        /// <summary>
        /// 三張同點
        /// </summary>
        Triple = 3,
        /// <summary>
        /// 順子：五張連續點數
        /// </summary>
        Straight = 4,
        /// <summary>
        /// 同花：五張相同花色
        /// </summary>
        Flush = 5,
        /// <summary>
        /// 葫蘆：三同點+二同點
        /// </summary>
        FullHouse = 6,
        /// <summary>
        /// 鐵支：四同點+任一
        /// </summary>
        FourOfAKing = 8,
        /// <summary>
        /// 同花順：4 + 5 牌型
        /// </summary>
        StraightFlush = 8,
    }
}