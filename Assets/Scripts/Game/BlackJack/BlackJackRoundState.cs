using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// [狀態機]表示21點牌局當下的流程階段
    /// </summary>
    public enum BlackJackRoundState
    {
        /// <summary>
        /// 預備開始新局
        /// </summary>
        WaitingForRound,
        /// <summary>
        /// 等待玩家要牌或停牌
        /// </summary>
        PlayerTurn,
        /// <summary>
        /// 玩家停牌後，荷官行動
        /// </summary>
        DealerTurn,
        /// <summary>
        /// 本局終局鎖定狀態
        /// </summary>
        Complete
    }
}

