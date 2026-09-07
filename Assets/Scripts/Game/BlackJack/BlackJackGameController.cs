using PokerGame.Betting;
using PokerGame.Core;
using PokerGame.Game;
using PokerGame.View;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField]
        private Button _startBtn;
        [SerializeField]
        private Button _betAddBtn;
        [SerializeField]
        private Button _betReduceBtn;
        [SerializeField]
        private Button _hitBtn;
        [SerializeField]
        private Button _standBtn;
        [SerializeField]
        private Button _nextRoundBtn;
        #endregion UI元件

        #region 欄位
        [SerializeField]
        private Dealer _dealer;
        private TableSession _session;
        [SerializeField]
        private CardHandLayout _playerLayout;
        [SerializeField]
        private CardHandLayout _dealerLayout;
        private bool _canBetting = true;
        /// <summary>
        /// 最小下注金常數
        /// </summary>
        private const int _minAmount = 100;
        private int _betAmount = _minAmount;
        #endregion 欄位

        #region 私有欄位
        /// <summary>
        /// 回合控制資料
        /// </summary>
        private readonly BlackJackRound _round = new BlackJackRound();
        /// <summary>
        /// 雙方全部勝負組合與賠率資料(規則書)
        /// </summary>
        private readonly BlackJackRules _rules = new BlackJackRules();
        /// <summary>
        /// 同一局內玩家的手牌
        /// </summary>
        private BlackJackHand PlayerHand => _round.PlayerHand;
        private int PlayerPoints => PlayerHand.Points;
        /// <summary>
        /// 同一局內荷官的手牌
        /// </summary>
        private BlackJackHand DealerHand => _round.DealerHand;
        private int DealerPoints => DealerHand.Points;
        /// <summary>
        /// 建立荷官色略類型：StandOn17
        /// </summary>
        private readonly IDealerStrategy _dealerStrategy = new StandOn17();
        /// <summary>
        /// 取得玩家錢包數值捷徑
        /// </summary>
        private int _blance => _session.Betting.Balance;
        #endregion 私有欄位

        #region 生命週期
        void Start()
        {
            _session = TableSession.Instance;
            _blanceLabel.text = _blance.ToString();
            ReduceBet();//預設一注(減注=跑最小值)
            UpdateBtnUI();//啟動對應的UI
            UpdatePointsUI(_playerPointsLabel, PlayerPoints);
            UpdatePointsUI(_dealerPointseLabel, DealerPoints);
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
            DealTo(DealerHand, _dealerLayout, false);//荷官第一張蓋牌
            DealTo(PlayerHand, _playerLayout);
            DealTo(DealerHand, _dealerLayout);
            //下注解鎖
            _canBetting = true;

            if (_round.TryComplete())
            {//一拿到牌就獲勝：BlackJack
                UpdateBtnUI();//更新對應的UI
                UpdatePointsUI(_playerPointsLabel, PlayerPoints);
                UpdatePointsUI(_dealerPointseLabel, DealerPoints);
                _dealer.ShowUpAll();
                //清算
                ResolveAndSettle();
                return;
            }

            //Debug.Log($"玩家：{PlayerHand.Points}點");
            Debug.Log($"莊家：{DealerHand.Points}點");
            _round.TryStart();//正式啟動
            UpdateBtnUI();//更新對應的UI
            UpdatePointsUI(_playerPointsLabel, PlayerPoints);
        }
        /// <summary>
        /// 發牌至指定對象之手牌區
        /// </summary>
        public void DealTo(BlackJackHand hand, CardHandLayout layout, bool showUp = true)
        {
            //荷官發牌至指定手牌對象定位
            PlayingCard card = _dealer.DealTo(layout.Root, showUp);
            hand.Add(card);//資料納管
            layout.Refresh();//視覺更新
        }
        /// <summary>
        /// [UI按鈕]加注
        /// </summary>
        public void AddBet()
        {
            _betAmount = Math.Min(_blance, _betAmount + _minAmount);
            UpdateBetUI();
        }

        private void UpdateBetUI()
        {
            _betLabel.text = _betAmount.ToString();
        }

        /// <summary>
        /// [UI按鈕]減注
        /// </summary>
        public void ReduceBet()
        {
            if (_blance < _minAmount)
            {
                _betAmount = 0;
            }
            else
            {
                _betAmount = Math.Max(_minAmount, _betAmount - _minAmount);
            }
            UpdateBetUI();
        }
        /// <summary>
        /// [UI按鈕]確認下注金
        /// </summary>
        public void ApplyBetting()
        {
            _session.Betting.TryPlaceBet(_betAmount);
            _canBetting = false;
            _blanceLabel.text = _blance.ToString();
        }

        /// <summary>
        /// 玩家回合可操作：再要一張牌(回合判定是否爆牌)
        /// </summary>
        public void Hit()
        {
            if (!_round.CanPlayerAct) return;//避免非玩家可行動誤觸
            ApplyBetting();
            //發一張牌給玩家(資料)
            DealTo(PlayerHand, _playerLayout);
            //視覺
            UpdatePointsUI(_playerPointsLabel, PlayerPoints);
            //驗證(爆牌：荷官直接攤牌)
            if (_round.CheckBust() || _round.CheckPass5())
            {
                UpdatePointsUI(_dealerPointseLabel, DealerPoints);
                _dealer.ShowUpAll();
                //清算
                ResolveAndSettle();
            }
            UpdateBtnUI();//更新對應的UI
        }
        /// <summary>
        /// 玩家回合可操作：放棄加牌(進到荷官回合)
        /// </summary>
        public void Stand()
        {
            ApplyBetting();
            _round.TryStand();//狀態機改變
            UpdateBtnUI();//更新對應的UI
            RunDealerTurn();//荷官動作
        }

        /// <summary>
        /// 再來一回
        /// </summary>
        public void NextRound()
        {
            //回收卡牌(視覺)
            _dealer.CollectAll();
            //起新局(資料)
            _round.TryNewGame();
            UpdateBtnUI();//啟動對應的UI
            UpdatePointsUI(_playerPointsLabel, PlayerPoints);
            UpdatePointsUI(_dealerPointseLabel, DealerPoints);
            _betAmount = _minAmount;
            _betLabel.text = _betAmount.ToString();
            _blanceLabel.text = _blance.ToString();
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 執行荷官回合(策略)
        /// </summary>
        private void RunDealerTurn()
        {
            while (_dealerStrategy.ShouldHit(DealerHand))
            {//反覆取牌到策略不允許 或 過5關為止
                DealTo(DealerHand, _dealerLayout);
                _dealerLayout.Refresh();
                if (_round.CheckPass5()) break;//滿五張強制中斷
            }
            //荷官回合結束攤牌
            UpdatePointsUI(_dealerPointseLabel, DealerPoints);
            _dealer.ShowUpAll();
            //遊戲總結
            if (_round.TryComplete())
            {
                //清算
                ResolveAndSettle();
            }

            UpdateBtnUI();
        }

        private void ResolveAndSettle()
        {
            if (_canBetting) return;
            //核對勝負結果
            RoundResult result = _rules.Resolve(PlayerHand, DealerHand);
            //結算(回收或賠)
            int returned = _session.Betting.Settle(result);
            //刷新Bet & Blance
            _betLabel.text = $"{result.Reason} - {returned}";
            _blanceLabel.text = _blance.ToString();
        }
        /// <summary>
        /// 依照遊戲狀態機啟動對應的UI
        /// </summary>
        private void UpdateBtnUI()
        {//簡寫法?檢查物件是否存在.再執行相關操作
            //開始鈕?.物件.是否可見(指定狀態：回合準備中)
            _startBtn?.gameObject.SetActive(_round.State == BlackJackRoundState.WaitingForRound);
            //下注按鈕?.物件.是否可見(指定狀態：玩家回合 & 未HIT前還可加注)
            _betAddBtn?.gameObject.SetActive(_round.State == BlackJackRoundState.PlayerTurn && _canBetting);
            _betReduceBtn?.gameObject.SetActive(_round.State == BlackJackRoundState.PlayerTurn && _canBetting);
            //要牌/停牌?.物件.是否可見(指定狀態：玩家回合)
            _hitBtn?.gameObject.SetActive(_round.State == BlackJackRoundState.PlayerTurn);
            _standBtn?.gameObject.SetActive(_round.State == BlackJackRoundState.PlayerTurn);
            //再來一回?.物件.是否可見(指定狀態：遊戲結束)
            _nextRoundBtn?.gameObject.SetActive(_round.State == BlackJackRoundState.Complete);
        }
        /// <summary>
        /// 更新牌裝上對應角色的點數
        /// </summary>
        /// <param name="tmpText">UI元件</param>
        /// <param name="points">當下點數</param>
        private void UpdatePointsUI(TMP_Text tmpText, int points)
        {
            tmpText.text = points > 0 ? $"{points}p" : "";
        }
        #endregion 私有方法
    }
}

