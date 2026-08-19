using UnityEngine;
using TMPro;
using PokerGame.Core;

namespace PokerGame.View
{
    /// <summary>
    /// 用來建立卡牌視覺物件單元
    /// </summary>
    public class CardView : MonoBehaviour
    {
        #region UI元件
        [SerializeField]//強制將私有欄位顯示在編輯器
        private TMP_Text _suitLabel;
        [SerializeField]
        private TMP_Text _rankLabel;
        #endregion UI元件

        #region 公開方法
        /// <summary>
        /// 將視覺與資料同步(綁定)
        /// </summary>
        /// <param name="card">卡牌資料</param>
        public void Bind(PlayingCard card)
        {
            //更新文字
            _suitLabel.text = GetSuitText(card.Suit);
            _rankLabel.text = GetRankText(card.Rank);
            //改顏色
            _suitLabel.color = GetSuitColor(card.Suit);
            _rankLabel.color = GetSuitColor(card.Suit);
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 取得翻譯完成的數值文字
        /// </summary>
        /// <param name="rank">數值原始資料</param>
        /// <returns>翻譯好的值</returns>
        private string GetRankText(Rank rank)
        {
            //邏輯判斷式
            switch (rank)
            {
                case Rank.Ace: return "A";
                case Rank.Jack: return "J";
                case Rank.Queen: return "Q";
                case Rank.King: return "K";
            }
            //同等於 default
            return ((int)rank).ToString();
        }
        /// <summary>
        /// 取得翻譯完成的花色文字
        /// </summary>
        /// <param name="suit">花色原始資料</param>
        /// <returns>翻譯好的花色</returns>
        private string GetSuitText(Suit suit)
        {
            //邏輯判斷式：♠ ♥ ♣ ♦
            switch (suit)
            {
                case Suit.Clubs: return "♣";
                case Suit.Diamonds: return "♦";
                case Suit.Hearts: return "♥";
                case Suit.Spades: return "♠";
                default: return "";
            }
        }

        private Color GetSuitColor(Suit suit)
        {
            switch (suit)
            {
                //紅色
                case Suit.Diamonds:
                case Suit.Hearts: return Color.red;
                //黑色
                case Suit.Clubs:
                case Suit.Spades: return Color.black;
                //預設白
                default: return Color.white;
            }
        }
        #endregion 私有方法
    }
}
