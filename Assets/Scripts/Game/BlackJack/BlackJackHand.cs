using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// 21點的手牌資料(一位玩家一組)
    /// </summary>
    public class BlackJackHand
    {
        #region 欄位
        /// <summary>
        /// 手牌存放處(清單物件)
        /// </summary>
        private readonly List<PlayingCard> _cards = new List<PlayingCard>();
        #endregion 欄位

        #region 公開屬性
        
        /// <summary>
        /// 手牌點數總和
        /// </summary>
        public int Points => CalculateTotal();
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 將撲克牌加入手牌
        /// </summary>
        /// <param name="card">撲克牌資料</param>
        public void Add(PlayingCard card)
        {
            _cards.Add(card);//加新資料
        }
        /// <summary>
        /// 清空手牌資料(起新局)
        /// </summary>
        public void Clear()
        {
            _cards.Clear();
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

