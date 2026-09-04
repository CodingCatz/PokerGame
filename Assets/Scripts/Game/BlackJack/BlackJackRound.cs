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
        /// 新起局的狀態：輔助判斷秒勝(BlackJack)
        /// </summary>
        public bool NewRound => State == BlackJackRoundState.WaitingForRound;
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
        /// <returns>結果回傳</returns>
        public bool CheckBust()
        {
            if (IsPlayerBust)
            {
                return TryComplete();
            }
            return false;
        }
        /// <summary>
        /// 確認是否過五關
        /// </summary>
        /// <returns></returns>
        public bool CheckPass5()
        {
            if (PlayerHand.IsPass5 || DealerHand.IsPass5)
            {
                return TryComplete();
            }
            return false;
        }
        /// <summary>
        /// 嘗試完成牌局
        /// </summary>
        public bool TryComplete()
        {
            //開局也可能結束：寫的是不能觸發的精準條件
            if (NewRound && !PlayerHand.IsBlackJack) return false;

            State = BlackJackRoundState.Complete;
            //觸發清算
            return true;
        }
        /// <summary>
        /// 嘗試開新局(資料重置)
        /// </summary>
        public void TryNewGame()
        {
            State = BlackJackRoundState.WaitingForRound;
            PlayerHand.Clear();
            DealerHand.Clear();
        }
        #endregion 公開方法
    }
}