using PokerGame.Core;
using PokerGame.Game;
using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// 21點的遊戲流程主控程式
    /// </summary>
    public class BlackJackGameController : MonoBehaviour
    {
        #region UI元件
        #endregion UI元件

        #region 欄位
        [SerializeField]
        private Dealer _dealer;
        private TableSession _session;
        [SerializeField]
        private BlackJackHand _playerHand;
        [SerializeField]
        private BlackJackHand _dealerHand;
        #endregion 欄位

        #region 私有欄位

        #endregion 私有欄位

        #region 生命週期
        void Start()
        {
            _session = TableSession.Instance;
        }
        #endregion 生命週期

        #region 公開方法

        #endregion 公開方法
        /// <summary>
        /// 開局首輪
        /// </summary>
        public void StartRound()
        {
            //荷官開局
            _dealer.BeginRound();
            //清空手牌
            _playerHand.Clear();
            _dealerHand.Clear();

            DealTo(_playerHand);
            DealTo(_dealerHand);
            DealTo(_playerHand);
            DealTo(_dealerHand);

        }
        /// <summary>
        /// 發牌至指定對象之手牌區
        /// </summary>
        public void DealTo(BlackJackHand hand)
        {
            //荷官發牌至指定手牌對象定位
            PlayingCard card = _dealer.DealTo(hand.transform);
            hand.Add(card);
        }
        #region 私有方法

        #endregion 私有方法
    }
}

