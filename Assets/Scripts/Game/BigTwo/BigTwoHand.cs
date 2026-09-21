using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 大老二單一玩家手牌資料
    /// </summary>
    public class BigTwoHand : PokerHand
    {
        #region 私有欄位
        private readonly BigTwoCardComparer _comparer = new BigTwoCardComparer();
        #endregion 私有欄位

        #region 公開方法
        /// <summary>
        /// 基本的大小排序功能
        /// </summary>
        public override void Sort()
        {
            _cards.Sort(_comparer);
        }
        #endregion 公開方法
    }

}