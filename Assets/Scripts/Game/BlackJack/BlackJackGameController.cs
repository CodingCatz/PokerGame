using PokerGame.Core;
using PokerGame.Game;
using PokerGame.View;
using TMPro;
using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// 21點的遊戲流程主控程式
    /// </summary>
    public class BlackJackGameController : MonoBehaviour
    {
        #region UI元件
        [SerializeField]
        private TMP_Text _betLabel;
        [SerializeField]
        private TMP_Text _blanceLabel;
        [SerializeField]
        private TMP_Text _playerPointsLabel;
        [SerializeField]
        private TMP_Text _dealerPointseLabel;
        #endregion UI元件

        #region 欄位
        [SerializeField]
        private Dealer _dealer;
        private TableSession _session;
        [SerializeField]
        private CardHandLayout _playerLayout;
        [SerializeField]
        private CardHandLayout _dealerLayout;
        #endregion 欄位

        #region 私有欄位
        /// <summary>
        /// 回合控制資料
        /// </summary>
        private readonly BlackJackRound _round = new BlackJackRound();
        /// <summary>
        /// 同一局內玩家的手牌
        /// </summary>
        private BlackJackHand PlayerHand => _round.PlayerHand;
        /// <summary>
        /// 同一局內荷官的手牌
        /// </summary>
        private BlackJackHand DealerHand => _round.DealerHand;
        #endregion 私有欄位

        #region 生命週期
        void Start()
        {
            _session = TableSession.Instance;
            StartRound();
        }
        #endregion 生命週期

        #region 公開方法
        /// <summary>
        /// 開局首輪
        /// </summary>
        public void StartRound()
        {
            //荷官開局
            _dealer.BeginRound();

            DealTo(PlayerHand, _playerLayout);
            DealTo(DealerHand, _dealerLayout);
            DealTo(PlayerHand, _playerLayout);
            DealTo(DealerHand, _dealerLayout);

            Debug.Log($"玩家：{PlayerHand.Points}點");
            Debug.Log($"莊家：{DealerHand.Points}點");
        }
        /// <summary>
        /// 發牌至指定對象之手牌區
        /// </summary>
        public void DealTo(BlackJackHand hand, CardHandLayout layout)
        {
            //荷官發牌至指定手牌對象定位
            PlayingCard card = _dealer.DealTo(layout.Root);
            hand.Add(card);//資料納管
            layout.Refresh();//視覺更新
        }
        #endregion 公開方法

        #region 私有方法

        #endregion 私有方法
    }
}

