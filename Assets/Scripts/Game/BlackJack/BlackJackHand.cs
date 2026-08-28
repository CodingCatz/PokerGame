using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BlackJack
{

    public class BlackJackHand : MonoBehaviour
    {
        #region 欄位
        /// <summary>
        /// 手牌存放處(清單物件)
        /// </summary>
        private readonly List<PlayingCard> _cards = new List<PlayingCard>();
        #endregion 欄位

        #region 公開方法
        /// <summary>
        /// 將撲克牌加入手牌
        /// </summary>
        /// <param name="card">撲克牌資料</param>
        public void Add(PlayingCard card)
        {
            _cards.Add(card);
        }
        /// <summary>
        /// 清空手牌資料(起新局)
        /// </summary>
        public void Clear()
        {
            _cards.Clear();
        }
        #endregion 公開方法
    }
}

