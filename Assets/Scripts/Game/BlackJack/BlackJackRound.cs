using PokerGame.View;
using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// 
    /// </summary>
    public class BlackJackRound
    {
        #region 公開屬性
        /// <summary>
        /// 取得目前遊戲回合的狀態
        /// </summary>
        public BlackJackRoundState State { get; private set; } = BlackJackRoundState.WaitingForRound;
        /// <summary>
        /// 取得玩家手牌資料
        /// </summary>
        public BlackJackHand PlayerHand { get; } = new BlackJackHand();
        /// <summary>
        /// 取得荷官手牌資料
        /// </summary>
        public BlackJackHand DealerHand { get; } = new BlackJackHand();
        /// <summary>
        /// 取得當下玩家是否可以合法操作
        /// </summary>
        public bool CanPlayerAct => State == BlackJackRoundState.PlayerTurn;


        #endregion 公開屬性
    }
}