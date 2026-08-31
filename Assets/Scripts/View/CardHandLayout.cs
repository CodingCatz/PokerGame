using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.View
{
    /// <summary>
    /// [UI視覺布局]純粹的手牌子物件排列控制
    /// </summary>
    public class CardHandLayout : MonoBehaviour
    {
        #region 欄位
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
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 更新排列(視覺刷新)
        /// </summary>
        public void Refresh()
        {
            int index = Count - 1;//子物件的索引號碼
            //使用固定間隔倍率在X軸上移動視覺物件(以父物件為原始基礎點)
            transform.GetChild(index).position = transform.position + Vector3.right * _sortingSpace * index;
        }
        #endregion 公開方法
    }
}

