using System;
using System.Collections.Generic;

namespace PokerGame.Core
{
    /// <summary>
    /// 保存一副標準撲克牌(4花色各13總和52張)
    /// ※可重置、洗牌、抽取
    /// </summary>
    public class Deck
    {
        #region 私有欄位
        /// <summary>
        /// 保管卡牌資料的清單
        /// </summary>
        private readonly List<PlayingCard> _cards = new List<PlayingCard>();
        /// <summary>
        /// C#內建的隨機庫(多面骰)
        /// </summary>
        private readonly Random _random = new Random();
        /// <summary>
        /// 下一張抽出的序號
        /// </summary>
        private int _nextIndex;
        #endregion 私有欄位

        #region 公開屬性
        /// <summary>
        /// 殘餘撲克牌數量
        /// (呼叫參數時會即時更新算式結果)
        /// </summary>
        public int Remaining => _cards.Count - _nextIndex;
        /// <summary>
        /// 牌庫是否用盡
        /// </summary>
        public bool IsEmpty => Remaining == 0;
        #endregion 公開屬性

        #region 建構式
        public Deck()
        {
            CreateStandardCards();
            Reset();
        }

        #endregion 建構式

        #region 公開方法
        /// <summary>
        /// 重設牌庫使用狀態
        /// </summary>
        public void Reset()
        {
            _nextIndex = 0;
        }

        /// <summary>
        /// 執行洗牌(演算法)
        /// </summary>
        public void Shuffle()
        {
            //For迴圈(起始值；終點值；迭代值)
            for (int index = _cards.Count; index > 0; index--)
            {
                //抽取要交換的索引碼 = 多面骰(0, 未洗過的最大值)
                int swapIndex = _random.Next(0, index);
                //暫存牌庫中最後一張(未洗過)的牌
                PlayingCard tmpCard = _cards[index - 1];
                //抽出的牌放到最後
                _cards[index - 1] = _cards[swapIndex];
                //完成交換(原本的最後放到抽出得位置)
                _cards[swapIndex] = tmpCard;
            }
        }

        /// <summary>
        /// 抽牌
        /// </summary>
        /// <returns>一張牌</returns>
        public PlayingCard Draw()
        {
            return _cards[_nextIndex++];//先使用後增加
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

