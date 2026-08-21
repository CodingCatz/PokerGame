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
            _dealer.DealTo(_playerHand);
            _dealer.DealTo(_dealerHand);
        }
        #endregion 公開方法
    }
}

