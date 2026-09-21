using PokerGame.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 牌型組合驗牌員(工具)
    /// </summary>
    public class BigTwoCombinationEvaluator
    {
        #region 公開方法
        /// <summary>
        /// 嘗試組合配對
        /// </summary>
        /// <param name="cards">卡牌資料</param>
        /// <param name="play">輸出(out)：配對的紀錄</param>
        /// <returns>是否成功</returns>
        public bool TryEvaluate(IReadOnlyList<PlayingCard> cards, out BigTwoPlay play)
        {
            play = null;
            //未選 or 選4 都無法成組
            if (cards.Count < 1 || cards.Count == 4) return false;

            if (cards.Count == 5) return CheckEvaluateFive(cards, out play);

            //1張以上開始做組合驗證
            if (cards.Count == 1)
            {
                play = new BigTwoPlay(BigTwoCombinationType.Single, cards, (int)cards[0].Rank);
                return true;
            }
            //是否全部同點
            if (!AllRanksMatch(cards)) return false;//否

            BigTwoCombinationType type = cards.Count == 2 ?
                BigTwoCombinationType.Pair :
                BigTwoCombinationType.Triple;

            play = new BigTwoPlay(type, cards, (int)cards[0].Rank);
            return true;
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 5張牌的專屬檢驗區塊
        /// </summary>
        private bool CheckEvaluateFive(IReadOnlyList<PlayingCard> cards, out BigTwoPlay play)
        {
            play = null;
            //是否同花
            bool isFlush = AllSuitMatch(cards);
            //是否順子
            bool isStraight = BigTwoStraightRules.TryGetStraight(cards, out int strength);
            //記下所有的牌(值)出現幾次
            Dictionary<Rank, int> counts = CountRanks(cards);
            
            //驗同花順是否成立
            if (isFlush && isStraight)
            {
                play = new BigTwoPlay(BigTwoCombinationType.StraightFlush, cards, strength);
                return true;
            }

            //驗鐵支是否成立
            int fourRank = FindRankWithCount(counts, 4);
            if (fourRank > 0)
            {
                play = new BigTwoPlay(BigTwoCombinationType.FourOfAKing, cards, fourRank);
                return true;
            }

            //驗葫蘆是否成立
            int tripleRank = FindRankWithCount(counts, 3);
            int pairRank = FindRankWithCount(counts, 2);
            if (tripleRank > 0 && pairRank > 0)//3同和2同同時發生
            {
                play = new BigTwoPlay(BigTwoCombinationType.FullHouse, cards, tripleRank);
                return true;
            }

            //驗同花是否成立
            if (isFlush)
            {
                play = new BigTwoPlay(BigTwoCombinationType.Flush, cards, GetHighestRank(cards));
                return true;
            }

            //驗順子是否成立
            if (isStraight)
            {
                play = new BigTwoPlay(BigTwoCombinationType.Straight, cards, strength);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 查詢並用字典紀錄每個 RANK 值出現幾次
        /// </summary>
        private Dictionary<Rank, int> CountRanks(IReadOnlyList<PlayingCard> cards)
        {
            //使用字典<關鍵值,對應數據>：指定的RANK出現幾次(int)
            Dictionary<Rank, int> counts = new Dictionary<Rank, int>();

            foreach (var card in cards) 
            {
                //該數值第一次出現先初始化：設為0
                if (!counts.ContainsKey(card.Rank)) counts[card.Rank] = 0;
                //美出現一次：疊加 1 次
                counts[card.Rank]++;
            }

            return counts;
        }

        /// <summary>
        /// 用字典查詢符合的預期數量對應的 RANK
        /// </summary>
        private int FindRankWithCount(Dictionary<Rank, int>counts, int expectedCount)
        {
            foreach (KeyValuePair<Rank, int> item in counts) 
            {
                if (item.Value == expectedCount) 
                    return BigTwoCardComparer.GetRankStrength(item.Key);
            }
            return 0;
        }
        
        /// <summary>
        /// 取得牌組內最高的RANK
        /// </summary>
        private int GetHighestRank(IReadOnlyList<PlayingCard> cards)
        {
            int highestRank = 0;

            foreach (var card in cards) 
            {//遍歷：看到更高就記下
                int rank = BigTwoCardComparer.GetRankStrength(card.Rank);
                if (rank > highestRank) highestRank = rank;
            }

            return highestRank;
        }

        /// <summary>
        /// 確認清單內所有牌花色都與第一張相同
        /// </summary>
        /// <param name="cards">卡牌清單</param>
        /// <returns>是否全同</returns>
        private bool AllSuitMatch(IReadOnlyList<PlayingCard> cards)
        {
            for (int i = 1; i < cards.Count; i++)
            {//只要查到一張不同就失敗
                if (cards[i].Suit != cards[0].Suit) return false;
            }
            return true;//完全相同
        }
        /// <summary>
        /// 確認清單內所有牌點數都與第一張相同
        /// </summary>
        /// <param name="cards">卡牌清單</param>
        /// <returns>是否全同</returns>
        private bool AllRanksMatch(IReadOnlyList<PlayingCard> cards)
        {
            for (int i = 1; i < cards.Count; i++)
            {//只要查到一張不同就失敗
                if (cards[i].Rank != cards[0].Rank) return false;
            }
            return true;//完全相同
        }
        #endregion 私有方法
    }
}