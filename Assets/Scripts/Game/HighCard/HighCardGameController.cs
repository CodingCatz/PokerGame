using PokerGame.Betting;
using PokerGame.Core;
using PokerGame.Game;
using System;
using TMPro;
using UnityEngine;

namespace PokerGame.Game.HighCard
{
    /// <summary>
    /// 比大小的遊戲流程主控程式
    /// </summary>
    public class HighCardGameController : MonoBehaviour
    {
        #region UI元件
        [SerializeField]
        private TMP_Text _betLabel;
        [SerializeField]
        private TMP_Text _blanceLabel;
        #endregion UI元件

        #region 欄位
        [SerializeField]
        private Dealer _dealer;
        private TableSession _session;
        [SerializeField]
        private Transform _playerHand;
        [SerializeField]
        private Transform _dealerHand;
        private int _betAmount = 10;
        #endregion 欄位

        #region 私有欄位
        /// <summary>
        /// 該遊戲專屬持有的遊戲規則書
        /// </summary>
        private readonly HighCardRules _rules = new HighCardRules();
        /// <summary>
        /// 取得玩家錢包數值捷徑
        /// </summary>
        private int _blance => _session.Betting.Balance;
        #endregion 私有欄位

        #region 生命週期
        void Start()
        {
            _session = TableSession.Instance;
            UpdateBetUI();
            UpdateBlanceUI();
        }
        #endregion 生命週期

        #region 公開方法
        /// <summary>
        /// [UI按鈕]加注
        /// </summary>
        public void AddBet()
        {
            _betAmount = Math.Min(_blance, _betAmount + 10);
            UpdateBetUI();
        }
        /// <summary>
        /// [UI按鈕]減注
        /// </summary>
        public void ReduceBet()
        {
            _betAmount = Math.Max(10, _betAmount - 10);
            UpdateBetUI();
        }
        /// <summary>
        /// [UI按鈕]確認下注金
        /// </summary>
        public void ApplyBetting()
        {
            _session.Betting.TryPlaceBet(_betAmount);
            //UpdateBlanceUI();
            //原本的流程節奏該要先執行一次：看到餘額減少
            _dealer.CollectAll();
        }

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
            RoundResult result = _rules.Resolve(playCard, dealerCard);
            int returnChips = _session.Betting.Settle(result);
            UpdateBlanceUI();
            //Debug.Log($"{result.Reason} 獲得 {returnChips} 籌碼。");
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 更新下注金的UI
        /// </summary>
        private void UpdateBetUI()
        {
            _betLabel.text = _betAmount.ToString();
        }
        /// <summary>
        /// 更新玩家籌碼餘額的UI
        /// </summary>
        private void UpdateBlanceUI()
        {
            _blanceLabel.text = _blance.ToString();
        }
        #endregion 私有方法
    }
}

