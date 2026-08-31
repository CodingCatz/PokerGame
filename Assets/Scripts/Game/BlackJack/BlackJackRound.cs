using PokerGame.View;
using System;
using Unity.VisualScripting;
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
        /// <summary>
        /// 玩家爆牌
        /// </summary>
        public bool IsPlayerBust => PlayerHand.Points > 21;
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 回合正式開始
        /// </summary>
        public void TryStart()
        {//預防牌局起始狀態不完整
            if (PlayerHand.Count < 2 || DealerHand.Count < 2) return;
            State = BlackJackRoundState.PlayerTurn;
        }
        /// <summary>
        /// 玩家停牌，轉移行動權
        /// </summary>
        public void TryStand()
        {
            if (!CanPlayerAct) return;//避免非玩家可行動誤觸
            State = BlackJackRoundState.DealerTurn;
        }
        /// <summary>
        /// 確認是否爆牌
        /// </summary>
        public void CheckBust()
        {
            if (IsPlayerBust) State = BlackJackRoundState.Complete;
        }
        #endregion 公開方法
    }
}