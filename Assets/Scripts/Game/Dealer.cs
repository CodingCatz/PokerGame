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
        /// <param name="dest">目的地</param>
        /// <returns>卡牌資料</returns>
        public PlayingCard DealTo(Transform dest)
        {
            //抽出一張(資料)
            PlayingCard card = _deck.Draw();
            //抽出一張(空閒牌面)
            CardView view = viewPool.Rent();
            //丟到所屬手牌區(目的地)
            view.transform.SetParent(dest, false);
            //資料與視覺組合
            view.Bind(card);
            //紀錄已發出去的牌面實體(物件池回收參考清單)
            _activeViews.Add(view);

            //傳出去
            return card;
        }

        /// <summary>
        /// 將目前已發出去的牌面收回到物件池
        /// </summary>
        public void CollectAll()
        {
            foreach (CardView view in _activeViews) 
            {
                viewPool.Return(view);
            }
        }
        #endregion 公開方法

    }
}

