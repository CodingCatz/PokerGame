using System.Collections.Generic;
using PokerGame.Core;
using PokerGame.View;
using UnityEngine;

namespace PokerGame.Game
{
    /// <summary>
    /// 荷官：負責牌組資料和牌面的調度(資料+視覺整合)
    /// </summary>
    public class Dealer : MonoBehaviour
    {
        #region 公開欄位
        /// <summary>
        /// 交由荷官控制的牌面物件池
        /// </summary>
        public CardViewPool viewPool;
        #endregion 公開欄位

        #region 私有欄位
        /// <summary>
        /// 建立整副牌(資料)
        /// </summary>
        private readonly Deck _deck = new Deck();
        /// <summary>
        /// 資料對應的卡牌視覺(物件清單)
        /// </summary>
        private readonly List<CardView> _activeViews = new List<CardView>();
        #endregion 私有欄位

        #region 生命週期
        /// <summary>
        /// 喚醒：比Start更早被執行
        /// </summary>
        private void Awake()
        {
            //用來準備物件池
            viewPool.Initialize(52);
        }
        #endregion 生命週期

        #region 公開方法
        /// <summary>
        /// 開始回合：收回牌面、重新洗牌
        /// </summary>
        public void BeginRound()
        {
            _deck.Reset();
            _deck.Shuffle();
        }
        /// <summary>
        /// 發牌給某人
        /// </summary>
        /// <returns>卡牌資料</returns>
        public PlayingCard DealTo()
        {
            //抽出一張
            PlayingCard card = _deck.Draw();

            //傳出去
            return card;
        }
        #endregion 公開方法

    }
}

