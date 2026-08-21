using PokerGame.Core;
using PokerGame.Game;
using UnityEngine;

namespace PokerGame.Game.HighCard
{
    public class HighCardGameController : MonoBehaviour
    {
        #region 欄位
        [SerializeField]
        private Dealer _dealer;
        [SerializeField]
        private Transform _playerHand;
        [SerializeField]
        private Transform _dealerHand;
        #endregion 欄位

        #region 私有欄位
        /// <summary>
        /// 該遊戲專屬持有的遊戲規則書
        /// </summary>
        private readonly HighCardRules _rules = new HighCardRules();
        #endregion 私有欄位

        #region 生命週期
        void Start()
        {
            PlayRound();
        }
        #endregion 生命週期

        #region 公開方法
        /// <summary>
        /// 遊玩回合
        /// </summary>
        public void PlayRound()
        {
            //荷官開局
            _dealer.BeginRound();
            //發牌給參與者
            PlayingCard playCard = _dealer.DealTo(_playerHand);
            PlayingCard dealerCard = _dealer.DealTo(_dealerHand);
            //用規則書取得結果
            string result = _rules.Resolve(playCard, dealerCard);

            Debug.Log(result);
        }
        #endregion 公開方法
    }
}

