using PokerGame.Core;
using UnityEngine;

namespace PokerGame.Game.HighCard
{
    /// <summary>
    /// [裁判]比大小的遊戲規則
    /// </summary>
    public class HighCardRules
    {
        #region 公開方法
        /// <summary>
        /// 比較兩者點數後得到的結果
        /// </summary>
        /// <param name="A">對象A</param>
        /// <param name="B">對象B</param>
        public string Resolve(PlayingCard A, PlayingCard B)
        {
            //值翻譯
            int valA = GetValue(A);
            int valB = GetValue(B);
            string result = "";
            //如果 (A大於B) 印出大
            if (valA > valB)
            {
                result = "大";
            }
            //否則 印出小
            else 
            {
                result = "小";
            }

            //比對結果
            return $"{A.Info()} 比 {B.Info()} {result}";
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 資料轉譯 (值放到十位數以上 + 花放置個位數 = 獨一無二數值)
        /// </summary>
        /// <param name="card">翻譯對象</param>
        /// <returns>回傳翻譯結果</returns>
        private int GetValue(PlayingCard card)
        {
            return ((int)card.Rank) * 10 + (int)card.Suit;
        }
        #endregion 私有方法

    }
}

