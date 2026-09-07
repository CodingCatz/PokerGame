using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// 21點的手牌資料(一位玩家一組)
    /// </summary>
    public class BlackJackHand : PokerHand
    {
        #region 公開屬性
        /// <summary>
        /// 手牌點數總和
        /// </summary>
        public int Points => CalculateTotal();
        /// <summary>
        /// 自然形成的21點(一發入魂)
        /// </summary>
        public bool IsBlackJack => Count == 2 && Points == 21;
        /// <summary>
        /// 過五關(五龍)
        /// </summary>
        public bool IsPass5 => Count == 5 && Points <= 21;
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 將撲克牌加入手牌
        /// </summary>
        /// <param name="card">撲克牌資料</param>
        public override void Add(PlayingCard card)
        {
            base.Add(card);
            Debug.Log("ADD測試");
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 計算手牌點數總和
        /// </summary>
        /// <returns></returns>
        private int CalculateTotal()
        {
            int total = 0;
            bool gotA = false;
            //遍歷手牌的RANK取得換算後的點數，加總至total
            foreach (PlayingCard card in _cards)
            {
                total += GetDefaultCardValue(card);
                //手牌內是否含有A
                if (card.Rank == Rank.Ace) gotA = true;
            }
            //總和在11以下(安全範圍內加了不爆) 並且拿到ACE，就+10點
            return total <= 11 && gotA ? total + 10 : total;
        }
        /// <summary>
        /// 21點的點數預設規則
        /// </summary>
        /// <param name="card"></param>
        /// <returns>換算輸出數值</returns>
        private int GetDefaultCardValue(PlayingCard card)
        {
            return card.Rank >= Rank.Ten ? 10 : (int)card.Rank;
        }
        #endregion 私有方法
    }
}

