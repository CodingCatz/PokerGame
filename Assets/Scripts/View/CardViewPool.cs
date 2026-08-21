using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PokerGame.View
{
    /// <summary>
    /// 牌面物件池：建立集中、租用與歸還的管理系統
    /// </summary>
    public class CardViewPool : MonoBehaviour
    {
        #region Unity欄位
        [SerializeField]//卡牌面預製物件
        [Header("牌面元件/預製物")]
        private CardView _cardPrefab;
        #endregion Unity欄位

        #region 私有欄位
        /// <summary>
        /// 用隊列的方式管理物件池：先進先出
        /// </summary>
        private readonly Queue<CardView> _cardViews = new Queue<CardView>();
        #endregion 私有欄位

        #region 公開方法
        /// <summary>
        /// 初始化物件池
        /// </summary>
        /// <param name="size">尺寸</param>
        public void Initialize(int size)
        {
            for (int i = 0; i < size; i++)
            {//依照尺寸執行圈數
                //具現化物件到指定的父物件下
                CardView tmpView = Instantiate(_cardPrefab, transform);
                //先隱藏：遊戲物件.設為(不可見)
                tmpView.gameObject.SetActive(false);
                //收納入池(入隊)
                _cardViews.Enqueue(tmpView);
            }
        }

        /// <summary>
        /// 租用一個空閒的牌面(資料顯示器)
        /// </summary>
        /// <returns>空閒的牌面(預製物)</returns>
        public CardView Rent()
        {
            //抽出一張牌(出列)
            CardView tmpView = _cardViews.Dequeue();
            //取消隱藏：遊戲物件.設為(可見)
            tmpView.gameObject.SetActive(true);
            return tmpView;
        }

        /// <summary>
        /// 回收一個使用過的牌面(資料顯示器)
        /// </summary>
        /// <param name="view">使用過的牌面</param>
        public void Return(CardView view)
        {
            //回歸到物件池管理的子物件
            view.transform.SetParent(transform, false);
            //先隱藏：遊戲物件.設為(不可見)
            view.gameObject.SetActive(false);
            //收回入池(重新入隊)
            _cardViews.Enqueue(view);
        }
        #endregion 公開方法
    }
}
