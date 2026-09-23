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
        #endregion 私有欄位

        #region 公開屬性
        /// <summary>
        /// 取得回合控制資料
        /// </summary>
        public BigTwoRound Round => _round;
        /// <summary>
        /// 是否為玩家(真人)回合
        /// </summary>
        public bool IsHumanRound => _round.CurrentPlayerIndex == 0;
        #endregion 公開屬性

        #region 公開方法

        public int Start(IReadOnlyList<BigTwoHand> hands) 
        {
            if (hands == null) return -1;
            //連結遊戲控制器上的手牌
            _hands = hands;
            //尋找開局玩家
            int openingPlayer = FindOpeningPlayer(hands);
            //設定開始的玩家序列號
            _round.StartPlayer(openingPlayer);
            return openingPlayer;
        }

        public bool CanPlay(int playerIndex, IReadOnlyList<PlayingCard> cards, out BigTwoPlay play)
        {
            return _evaluator.TryEvaluate(cards, out play)
                && _round.CanPlay(playerIndex, play);
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
        #endregion 私有方法
    }
}