using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 依牌型比對大小強度的大老二牌組
    /// </summary>
    public class BigTwoPlayComparer : IComparer<BigTwoPlay>
    {
        /// <summary>
        /// 左：挑戰者 VS 右：衛冕者(牌桌有可能空)
        /// </summary>
        /// <returns></returns>
        public int Compare(BigTwoPlay left, BigTwoPlay right)
        {
            if (left == null) return -1;//出牌者空手：不合法
            if (right == null) return 1;//比對目標空：成立
            if (left.Count != right.Count) return -1;//出牌張數對不上：不合法
            //牌型勝負驗證邏輯
            int typeCompare = left.Type.CompareTo(right.Type);
            if (typeCompare != 0) return typeCompare;
            //牌型平手的驗證邏輯
            return left.Strength.CompareTo(right.Strength);
        }

        /// <summary>
        /// 驗證挑戰者是否能戰勝衛冕者(前提張數相同)
        /// </summary>
        /// <returns></returns>
        public bool CanBeat(BigTwoPlay left, BigTwoPlay right)
        {
            return left.Count == right.Count && Compare(left, right) > 0;
        }
    }
}