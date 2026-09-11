using PokerGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokerGame.View
{
    /// <summary>
    /// 手牌視覺排列方式
    /// </summary>
    public enum AlignmentMode
    {
        /// <summary>
        /// 往左發展
        /// </summary>
        Left = -1,
        /// <summary>
        /// 置中(左右發展，實際上起點往左偏)
        /// </summary>
        Center = 0,
        /// <summary>
        /// 往右發展
        /// </summary>
        Right = 1,
    }

    /// <summary>
    /// [UI視覺布局]純粹的手牌子物件排列控制
    /// </summary>
    public class CardHandLayout : MonoBehaviour
    {
        #region 欄位
        /// <summary>
        /// 排列模式
        /// </summary>
        [SerializeField]
        private AlignmentMode _mode;
        /// <summary>
        /// 卡牌排列間隔
        /// </summary>
        [Range(0.5f, 1.5f)]
        [SerializeField]
        private float _sortingSpace = 1.5f;
        #endregion 欄位

        #region 公開屬性
        /// <summary>
        /// [手牌根物件]發牌時的父物件定位
        /// </summary>
        public Transform Root => transform;
        /// <summary>
        /// 擁有的手牌數
        /// </summary>
        public int Count => Root.childCount;
        /// <summary>
        /// 排列方向
        /// </summary>
        public int Direction => _mode == AlignmentMode.Left ? -1 : 1;  
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 更新排列(視覺刷新)
        /// </summary>
        public void Refresh()
        {
            int index = Count - 1;//子物件的索引號碼
            //更新所有的排列位置
            for (int i = 0; i < Count; i++)
            {
                //使用固定間隔倍率在X軸上移動視覺物件(以父物件為原始基礎點)
                transform.GetChild(i).position =
                    //定位原點(加上偏移)
                    transform.position + Offset(index) +
                    //偏移方向(可能因為Mode.Left轉負)
                    Direction * transform.right * _sortingSpace * i;
            }
            
        }
        /// <summary>
        /// 重新綁定資料和視覺(資料次序有變化時，視覺需要同步更新)
        /// </summary>
        /// <param name="cards"></param>
        public void ReBindCards(IReadOnlyList<PlayingCard> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {//逐一重綁資料和視覺元件
                transform.GetChild(i).GetComponent<CardView>().Bind(cards[i]);
            }
        }
        /// <summary>
        /// 選取上浮視覺提示功能
        /// </summary>
        /// <param name="selectionIndexs">選取卡牌索引號清單</param>
        public void SelectionToggle(IReadOnlyList<int> selectionIndexs)
        {
            for (int i = 0; i < Count; i++)
            {//全部子物件過一次檢查
                SelectionPop(transform.GetChild(i), selectionIndexs.Contains(i));
            } 
        }
        #endregion 公開方法

        #region 私有功能
        /// <summary>
        /// 給Center專用
        /// (向左偏移起點，分一半的總長到左邊)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 Offset(int index)
        {
            return _mode == AlignmentMode.Center ?
                //左邊 * 一半的總長 * 間格倍率
                (-1 * transform.right) * (index / 2) * _sortingSpace : Vector3.zero;
        }
        /// <summary>
        /// 更新選取卡牌的POP位置
        /// </summary>
        /// <param name="transform">卡牌的Transform</param>
        /// <param name="isSelected">是否為選取對象</param>
        private void SelectionPop(Transform transform, bool isSelected)
        {
            Vector3 pos = transform.localPosition;//紀錄原點(基於父物件)
            pos.y = isSelected ? 1 : 0;//是否選取：往上 或 不動(歸零)
            transform.localPosition = pos;//重新定位
        }
        #endregion 私有功能
    }
}

