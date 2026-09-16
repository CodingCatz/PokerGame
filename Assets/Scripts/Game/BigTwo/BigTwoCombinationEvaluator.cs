using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 牌型組合驗牌員(工具)
    /// </summary>
    public class BigTwoCombinationEvaluator
    {
        #region 公開方法
        public bool TryEvaluate(IReadOnlyList<PlayingCard> cards)
        {
            //未選 or 選4 都無法成組
            if (cards.Count < 1 || cards.Count == 4) return false;
            //1張以上開始做組合驗證
            if (cards.Count == 1)
            {

                return true;
            }
            //是否全部同點
            if (!AllRanksMatch(cards)) return false;//否
            BigTwoCombinationType type = cards.Count == 2 ?
                BigTwoCombinationType.Pair :
                BigTwoCombinationType.Triple;
            return true;
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 確認清單內所有牌點數都與第一張相同
        /// </summary>
        /// <param name="cards">卡牌清單</param>
        /// <returns>是否全同</returns>
        private bool AllRanksMatch(IReadOnlyList<PlayingCard> cards)
        {
            for (int i = 1; i < cards.Count; i++)
            {//只要查到一張不同就失敗
                if (cards[i].Rank != cards[0].Rank) return false;
            }
            return true;//完全相同
        }
        #endregion 私有方法
    }
}