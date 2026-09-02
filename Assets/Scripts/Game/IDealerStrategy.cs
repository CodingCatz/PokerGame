using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// [介面]定義荷官要牌策略
    /// </summary>
    public interface IDealerStrategy
    {
        #region 功能定義
        /// <summary>
        /// 依照目前狀況決定是否拿一張牌
        /// </summary>
        /// <returns>是/否拿牌</returns>
        bool ShouldHit(BlackJackHand hand);
        #endregion 功能定義
    }
}

