using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 玩家選取牌的暫存
    /// </summary>
    public class BigTwoSelection
    {
        #region 私有欄位
        /// <summary>
        /// 選取的卡牌清單
        /// </summary>
        private List<PlayingCard> _cards = new List<PlayingCard>();
        #endregion 私有欄位

        #region 公開屬性
        /// <summary>
        /// 選取的張數
        /// </summary>
        public int Count => _cards.Count;
        /// <summary>
        /// 供對外唯讀的接口
        /// </summary>
        public IReadOnlyList<PlayingCard> Cards => _cards;
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 切換卡牌的選取狀態紀錄
        /// </summary>
        /// <param name="card">執行對象，點選的卡</param>
        public bool Toggle(PlayingCard card)
        {
            if (card == null) return false;
            //點選的卡牌是否包含於已選清單內(就會成功移除)
            if (_cards.Remove(card)) return true;
            if (_cards.Count >= 5) return false;
            _cards.Add(card);//若無：就加入(選取)
            return true;
        }
        /// <summary>
        /// 清除所有選取
        /// </summary>
        public void Clear()
        {
            _cards.Clear();
        }
        #endregion 公開方法
    }
}