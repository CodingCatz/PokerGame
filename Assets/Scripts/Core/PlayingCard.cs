namespace PokerGame.Core
{
    /// <summary>
    /// [類別]一張撲克牌的資料結構
    /// </summary>
    public class PlayingCard
    {
        #region 公開屬性
        /// <summary>
        /// [唯讀]取得花色
        /// </summary>
        public Suit Suit { get; }
        /// <summary>
        /// [唯讀]取得點數
        /// </summary>
        public Rank Rank { get; }
        #endregion 公開屬性

        #region 建構式
        /// <summary>
        /// 建立一張撲克牌
        /// </summary>
        /// <param name="suit">花色</param>
        /// <param name="rank">點數</param>
        public PlayingCard(Suit suit, Rank rank)
        {
            Suit = suit;//紀錄在上方的欄位中
            Rank = rank;
        }
        #endregion 建構式

        #region 公開方法
        /// <summary>
        /// 撲克牌資料的文字版
        /// </summary>
        /// <returns>花色+數值的文字</returns>
        public string Info()
        {
            return Suit.ToString() + (int)Rank;
        }
        #endregion 公開方法
    }
}