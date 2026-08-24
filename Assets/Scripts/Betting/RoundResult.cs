using UnityEngine;

namespace PokerGame.Betting
{
    /// <summary>
    /// [資料結構]提供下注系統結算一回中的所有結果
    /// </summary>
    public class RoundResult
    {
        #region 公開屬性
        /// <summary>
        /// 取得玩家的勝負
        /// </summary>
        public RoundOutcome Outcome { get; }
        /// <summary>
        /// 取得下注倍率
        /// </summary>
        public float ReturnRate { get; }
        /// <summary>
        /// 取得輸出這筆結果的原因(文字紀錄)
        /// </summary>
        public string Reason { get; }
        #endregion 公開屬性

        #region 建構式
        /// <summary>
        /// 建立一筆回合產生的結果
        /// </summary>
        public RoundResult(RoundOutcome outcome, float returnRate, string reson)
        {
            Outcome = outcome;
            ReturnRate = returnRate;
            Reason = reson;
        }
        #endregion 建構式

        #region 公開方法
        /// <summary>
        /// 依下注計算回報籌碼(整數)
        /// </summary>
        /// <param name="bet">下注值</param>
        /// <returns>結算回報</returns>
        public int CalculateReturn(int bet)
        {
            return (int)(bet * ReturnRate);
        }
        #endregion 公開方法
    }
}
