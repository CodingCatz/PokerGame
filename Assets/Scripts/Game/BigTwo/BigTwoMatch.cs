using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 玩家間手牌協調規則(主要包含梅花三先行)
    /// </summary>
    public class BigTwoMatch
    {
        #region 私有欄位
        /// <summary>
        /// 手牌資料連結
        /// </summary>
        private IReadOnlyList<BigTwoHand> _hands;
        /// <summary>
        /// 牌型進化檢驗器
        /// </summary>
        private readonly BigTwoCombinationEvaluator _evaluator = new BigTwoCombinationEvaluator();
        /// <summary>
        /// 回合流程控制器
        /// </summary>
        private readonly BigTwoRound _round = new BigTwoRound();

        private bool _openingRequired = false;
        #endregion 私有欄位

        #region 公開屬性
        /// <summary>
        /// 取得回合控制資料
        /// </summary>
        public BigTwoRound Round => _round;
        /// <summary>
        /// 是否已連上四家手牌
        /// </summary>
        public bool IsStarted => _hands != null;
        /// <summary>
        /// 是否處於可完成輪次回合的狀態
        /// </summary>
        public bool IsComplete => WinnerIndex >= 0;
        /// <summary>
        /// 遊戲於可運行狀態
        /// </summary>
        public bool IsGaming => IsStarted && !IsComplete;
        /// <summary>
        /// 是否為玩家(真人)回合
        /// </summary>
        public bool IsHumanRound => IsGaming && _round.CurrentPlayerIndex == 0;
        
        /// <summary>
        /// 當前的領先者序號
        /// </summary>
        public int WinnerIndex { get; private set; } = -1;
        

        #endregion 公開屬性

        #region 公開方法

        public int Start(IReadOnlyList<BigTwoHand> hands) 
        {
            Reset();
            if (hands == null) return -1;
            //連結遊戲控制器上的手牌
            _hands = hands;
            //開局條件：Y
            _openingRequired = true;
            //尋找開局玩家
            int openingPlayer = FindOpeningPlayer(hands);
            //設定開始的玩家序列號
            _round.StartPlayer(openingPlayer);
            return openingPlayer;
        }

        public bool CanPlay(int playerIndex, IReadOnlyList<PlayingCard> cards, out BigTwoPlay play)
        {
            if (!_evaluator.TryEvaluate(cards, out play)) return false;
            //不是開局者就要包含有開局牌
            return (!_openingRequired || ContainsOpeningCard(cards))
            && _round.CanPlay(playerIndex, play);
        }

        /// <summary>
        /// 驗證後再提交
        /// </summary>
        /// <returns>是否合格</returns>
        public bool TryPlay(int playerIndex, IReadOnlyList<PlayingCard> cards, out BigTwoPlay play)
        {
            if (!CanPlay(playerIndex, cards, out play) || 
                !_round.TryPlay(playerIndex, play)) return false;
            //手牌脫離
            foreach (PlayingCard card in play.Cards) _hands[playerIndex].Remove(card);
            _openingRequired = false;
            //離手後手牌空：產生勝利者
            if (_hands[playerIndex].Count == 0) WinnerIndex = playerIndex;
            return true;
        }

        /// <summary>
        /// 該員放棄出牌
        /// </summary>
        /// <returns></returns>
        public bool TryPass(int playerIndex, out bool clearedTable)
        {
            clearedTable = false;
            return !IsComplete && _round.TryPass(playerIndex, out clearedTable);
        }

        public void Reset()
        {
            _hands = null;
            WinnerIndex = -1;
            _round.StartPlayer(0);
            _openingRequired = false;
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 尋找持有梅花三的開局玩家
        /// </summary>
        private int FindOpeningPlayer(IReadOnlyList<BigTwoHand> hands)
        {
            for (int i = 0; i < hands.Count; i++)
            {//為了定位玩家號碼
                foreach (PlayingCard card in hands[i].Cards)
                {//檢查手牌中有無梅花三
                    if (card.Suit == Suit.Clubs && card.Rank == Rank.Three)
                        return i;//回傳定位的玩家序列號
                }
            }
            return -1;
        }

        /// <summary>
        /// 第一手必須包含梅花三，其他首出不限制
        /// </summary>
        private bool ContainsOpeningCard(IReadOnlyList<PlayingCard> cards)
        {
            foreach (PlayingCard card in cards)
                if (card.Suit == Suit.Clubs && card.Rank == Rank.Three) return true;
            return false;
        }
        #endregion 私有方法
    }
}