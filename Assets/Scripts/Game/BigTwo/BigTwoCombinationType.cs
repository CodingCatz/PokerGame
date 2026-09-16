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
    }
}