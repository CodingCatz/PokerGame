using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game
{
    /// <summary>
    /// [抽象層]手牌底層類別
    /// </summary>
    public abstract class PokerHand
    {
        #region 欄位
        /// <summary>
        /// 手牌存放處(清單物件)
        /// </summary>
        protected readonly List<PlayingCard> _cards = new List<PlayingCard>();
        #endregion 欄位

        #region 公開屬性
        /// <summary>
        /// 手牌持有數量
        /// </summary>
        public int Count => _cards.Count;
        /// <summary>
        /// 供對外唯讀的接口
        /// </summary>
        public IReadOnlyList<PlayingCard> Cards => _cards;
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// [虛擬]將撲克牌加入手牌
        /// </summary>
        /// <param name="card">撲克牌資料</param>
        public virtual void Add(PlayingCard card)
        {
            _cards.Add(card);//加新資料
        }
        /// <summary>
        /// [虛擬]取得指定物件在目前排序索引號碼
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public virtual int IndexOf(PlayingCard card)
        {
            return _cards.IndexOf(card);
        }
        /// <summary>
        /// [虛擬]排序的功能
        /// </summary>
        public virtual void Sort()
        {
            //預留
        }
        /// <summary>
        /// [虛擬]將撲克牌移出手牌
        /// </summary>
        /// <param name="card">撲克牌資料</param>
        public virtual void Remove(PlayingCard card)
        {
            _cards.Remove(card);//移除資料
        }
        /// <summary>
        /// [虛擬]清空手牌資料(起新局)
        /// </summary>
        public virtual void Clear()
        {
            _cards.Clear();
        }
        #endregion 公開方法
    }
}