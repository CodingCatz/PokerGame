using System;
using System.Collections.Generic;

namespace PokerGame.Core
{
    /// <summary>
    /// 保存一副標準撲克牌(4花色各13總和52張)
    /// </summary>
    public class Deck
    {
        #region 私有欄位
        /// <summary>
        /// 保管卡牌資料的清單
        /// </summary>
        private readonly List<PlayingCard> _cards = new List<PlayingCard>();
        /// <summary>
        /// 下一張抽出的序號
        /// </summary>
        private int _nextIndex;
        #endregion 私有欄位

        #region 公開屬性

        #endregion 公開屬性

        #region 建構式
        public Deck()
        {
            CreateStandardCards();
        }

        #endregion 建構式

        #region 公開方法
        /// <summary>
        /// 重設
        /// </summary>
        public void Reset()
        {
            _nextIndex = 0;
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        public void Shuffle()
        {

        }

        /// <summary>
        /// 抽牌
        /// </summary>
        /// <returns>一張牌</returns>
        public PlayingCard Draw()
        {
            //_nextIndex++; 先跳號
            return _cards[_nextIndex++];//先使用後增加
            _nextIndex++;//跳一號不會執行
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 創建一組標準的卡牌
        /// </summary>
        private void CreateStandardCards()
        {
            //遍歷四個花色 (宣告單體來裝抽出的資料 in 整包列舉內(描述型別))
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                //遍歷十三個點數 (宣告單體 in 整包列舉內(描述型別))
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    //記錄到清單上：新建 卡牌實體(花,值);
                    _cards.Add(new PlayingCard(suit, rank));
                }
            }
        }
        #endregion 私有方法
    }
}

