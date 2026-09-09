using PokerGame.Core;
using System.Collections.Generic;
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
                    //篇移方向(可能因為Mode.Left轉負)
                    Direction * transform.right * _sortingSpace * i;
            }
            
        }
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
        #endregion 公開方法
    }
}

