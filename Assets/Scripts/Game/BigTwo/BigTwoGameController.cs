using PokerGame.Core;
using PokerGame.View;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 大老案的遊戲流程控制：發牌到4位玩家
    /// </summary>
    public class BigTwoGameController : PokerGameBehaviour
    {
        #region UI元件
        /// <summary>
        /// [UI]遊戲狀態文字
        /// </summary>
        [SerializeField]
        private TMP_Text _statusLabel;
        /// <summary>
        /// [UI]開始按鈕
        /// </summary>
        [SerializeField]
        private Button _startBtn;
        /// <summary>
        /// [UI]出牌按鈕
        /// </summary>
        [SerializeField]
        private Button _playBtn;
        /// <summary>
        /// [UI]出牌按鈕上的提醒文字
        /// </summary>
        [SerializeField]
        private TMP_Text _playLabel;
        /// <summary>
        /// [UI]放棄按鈕
        /// </summary>
        [SerializeField]
        private Button _passBtn;
        /// <summary>
        /// [UI]清除選取按鈕
        /// </summary>
        [SerializeField]
        private Button _clearBtn;
        /// <summary>
        /// [定位物件]廢牌區
        /// </summary>
        [SerializeField]
        private Transform _discardRoot;
        #endregion UI元件

        #region UI相關功能
        /// <summary>
        /// [UI按鈕]開始牌局
        /// </summary>
        public void StartGame()
        {
            //未回到待機 或 遊戲已在進行 不能開啟新局
            if (!Idle || _match.IsGaming) return;
            RunFlow(true);//啟動非同步行程(開局)
        }
        /// <summary>
        /// [UI按鈕]出牌(選取的)
        /// </summary>
        public void PlaySelectedCards()
        {
            if (!CanHumanAct) return;
            //記下牌的畫面索引
            List<int> indices = CaptureIndices(0, _selection.Cards);
            if (!_match.TryPlay(0, _selection.Cards, out BigTwoPlay play))
            {
                RefreshControls();
                return;
            }
            //實際提交
            ShowCommittedPlay(0, indices, play);
            ContinueGame();//遊戲繼續
        }
        /// <summary>
        /// [被委派/動態UI]每個CardView的點擊觸發
        /// </summary>
        /// <param name="view"></param>
        public void CardViewClick(CardView view)
        {
            if (!CanHumanAct || view == null) return;
            //手牌 Layout 被選中的視覺對應序號紀錄
            int index = _playerLayouts[0].IndexOf(view);
            if (index < 0) return;
            //執行選取紀錄
            _selection.Toggle(_hands[0].Cards[index]);
            RefreshSelectionView();
            //出牌鈕狀態更新
            RefreshControls();

        }
        /// <summary>
        /// [UI按鈕]放棄操作
        /// </summary>
        public void PassTurn()
        {
            if (!CanHumanAct || !_match.TryPass(0, out bool clearedTable)) return;
            _selection.Clear();
            //此輪PASS是最後一家，清桌回收廢牌(進廢牌區)
            if (clearedTable) _tableLayout.MoveAllTo(_discardRoot);
            //刷新桌面視覺
            RefreshSelectionView();
            RefreshControls();
            //遊戲要繼續
            ContinueGame();
        }

        /// <summary>
        /// [UI按鈕]取消選取的
        /// </summary>
        public void ClearSelection()
        {
            if (!CanHumanAct) return;
            _selection.Clear();
            RefreshSelectionView();
            RefreshControls();
        }
        /// <summary>
        /// 更新所有控制介面的狀態
        /// </summary>
        private void RefreshControls()
        {
            _startBtn?.gameObject.SetActive(Idle && (!_match.IsStarted || _match.IsComplete));
            bool canPlay = false;
            BigTwoPlay play = null;
            if (CanHumanAct) canPlay = _match.CanPlay(0, _selection.Cards, out play);
            _playBtn?.gameObject.SetActive(canPlay);
            //印出牌型
            if (play != null) _playLabel.text = play.Type.ToString();
            _passBtn?.gameObject.SetActive(CanHumanAct && _match.Round.TopPlay != null);
            _clearBtn?.gameObject.SetActive(CanHumanAct && _selection.Count > 0);
        }
        #endregion UI相關功能

        #region 欄位
        /// <summary>
        /// 遊戲的延遲時間(速度)，對外公開可調的接口
        /// </summary>
        [Range(0.1f, 2f)]//轉成有範圍的拉桿
        [SerializeField]
        private float _delayTime = 1f;
        /// <summary>
        /// 視覺排版(玩家們)
        /// </summary>
        [SerializeField]
        private CardHandLayout[] _playerLayouts;
        /// <summary>
        /// 視覺排版(桌面)
        /// </summary>
        [SerializeField]
        private CardHandLayout _tableLayout;
        /// <summary>
        /// 手牌資料
        /// </summary>
        private readonly List<BigTwoHand> _hands = new List<BigTwoHand>();
        /// <summary>
        /// 操作者的卡牌選取器(共用)
        /// </summary>
        private readonly BigTwoSelection _selection = new BigTwoSelection();
        
        /// <summary>
        /// 玩家間手牌協調檢查器
        /// </summary>
        private readonly BigTwoMatch _match = new BigTwoMatch();
        /// <summary>
        /// C#內建的執行序身分認證
        /// </summary>
        private CancellationTokenSource _flowCancellation;
        /// <summary>
        /// 遊戲是否可以開始進行操作
        /// </summary>
        private bool _ready = false;

        #endregion 欄位

        #region 公開屬性
        /// <summary>
        /// 實作介面上的遊戲名稱
        /// </summary>
        public override string GameName => "Big Two";
        /// <summary>
        /// 遊戲速率(毫秒延遲)
        /// </summary>
        public int GameSpeed => (int)(_delayTime * 1000);
        /// <summary>
        /// 無任何異步行程在執行中，遊戲處於閒置狀態
        /// </summary>
        public bool Idle => _flowCancellation == null;
        /// <summary>
        /// 真人操作時機
        /// </summary>
        public bool CanHumanAct => _ready && Idle && _match.IsHumanRound;
        #endregion 公開屬性

        #region 生命週期
        //建立手牌資料容器 * 4
        private void Awake() => EnsureHands();

        private void OnEnable() => EnterMode();
        //程式腳本被關閉時觸發(單純的停止遊戲資料的回收機制)
        private void OnDisable() => ExitMode();
        //場景被卸載(物件被銷毀)，徹底停止一些背景任務(執行序)
        private void OnDestroy() => CancelFlow();
        #endregion 生命週期

        #region 公開方法
        /// <summary>
        /// 2.按下開始後顯示發牌訊息 & 更新桌面視覺狀態
        /// </summary>
        public override void EnterMode()
        {
            //開始發牌(訊息)
            UpdateStatusUI("Start Deal Four Hands.");
            RefreshControls();
            //桌面(視覺)狀態刷新
            RefreshSelectionView();
        }

        public override void ExitMode()
        {
            //流程清除(中止背景任務)
            CancelFlow();
            //整理桌面(資料 & 視覺)
            ReleaseCrads();
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 1.顯示開始遊戲按鈕和資訊 & 確認(建立)四組玩家手牌容器
        /// </summary>
        private void EnsureHands()
        {
            UpdateStatusUI("Ready to Play ?");
            RefreshControls();
            for (int i = 0; i < _playerLayouts.Length; i++)
            {
                _hands.Add(new BigTwoHand());
            }
        }

        /// <summary>
        /// 2-0.遊戲流程控制：發牌、電腦玩家出牌、玩家出牌
        /// 非同步流程與電腦最小策略
        /// </summary>
        /// <param name="dealFirst">是否先發牌</param>
        private async void RunFlow(bool dealFirst)
        {
            if (_flowCancellation != null) return;
            var source = new CancellationTokenSource();//建立一個 TASK 的辨識碼(行程的ID)
            _flowCancellation = source;//行程開始
            RefreshControls();//刷新UI狀態

            if (dealFirst)
            {//發牌(資料 & 視覺)
                ReleaseCrads();
                _dealer.BeginRound();
                UpdateStatusUI("Start Deal Four Hands.");
                await StartDealAsync(GameSpeed, source.Token);
            }

            //電腦決策流程
            await RunComputerTurnsAsync(source.Token);
            ShowTurn();

            _flowCancellation = null;//行程結束
            source.Dispose();//釋放資源
            RefreshControls();//刷新UI狀態

        }

        /// <summary>
        /// 2-1.[非同步行程]開局的發牌行程
        /// </summary>
        /// <param name="msec">遊戲速度</param>
        private async Task StartDealAsync(int msec, CancellationToken token)
        {
            //各發13張到4家(※有可能不足4，以實際的hands數量為主)
            for (int round = 0; round < 13; round++)
            {
                for (int playerIndex = 0; playerIndex < _hands.Count; playerIndex++)
                {
                    //避免任務變成殭屍行程的安全機制
                    token.ThrowIfCancellationRequested();
                    //視覺實體+資料分配：各玩家
                    PlayingCard card = playerIndex == 0 ? //玩家本身
                        //幫他綁定 CardViewClick 卡牌被點擊時要做的事情
                        _dealer.DealTo(_playerLayouts[playerIndex].Root, CardViewClick) :
                        //其他 NPC 走原始流程
                        _dealer.DealTo(_playerLayouts[playerIndex].Root/*, playerIndex == 0*/);
                    _hands[playerIndex].Add(card);
                    //狀態更新
                    _playerLayouts[playerIndex].Refresh();
                    await Task.Delay(msec);//等待：延遲任務
                }
            }

            //2-2.手牌順序整理
            UpdateStatusUI("Players are Sorting.");
            for (int i = 0; i < _hands.Count; i++)
            {
                //避免任務變成殭屍行程的安全機制
                token.ThrowIfCancellationRequested();
                //資料排序
                _hands[i].Sort();
                //整理過的資料和視覺重綁+排版
                _playerLayouts[i].ReBindCards(_hands[i].Cards);
                await Task.Delay(msec);//等待：延遲任務
            }

            //2-3.手牌擁有梅花三的玩家開始出牌
            UpdateStatusUI("Owns Three of Clubs \n Player Start Play.");
            await Task.Delay(msec * 10);//等待：延遲任務
            //確定起始玩家(四家手牌巡檢)
            UpdateStatusUI($"Player {_match.Start(_hands) + 1} Play Cards.");

            //完全準備完畢
            _ready = true;
        }

        /// <summary>
        /// 電腦自動化流程
        /// </summary>
        private async Task RunComputerTurnsAsync(CancellationToken token)
        {
            while (!_match.IsComplete && !_match.IsHumanRound)
            {
                int player = _match.Round.CurrentPlayerIndex;
                UpdateStatusUI($"Player {player + 1} Thinking...");
                await Task.Delay(GameSpeed, token);
                token.ThrowIfCancellationRequested();
                List<PlayingCard> cards = FindComputerSingle(player);
                if (cards == null)
                {
                    _match.TryPass(player, out bool clearedTable);
                    if (clearedTable) _tableLayout.MoveAllTo(_discardRoot);
                    continue;
                }
                List<int> indices = CaptureIndices(player, cards);
                if (!_match.TryPlay(player, cards, out BigTwoPlay play))
                    throw new InvalidOperationException("電腦出牌失敗。");
                ShowCommittedPlay(player, indices, play);
            }
        }

        /// <summary>
        /// [簡易模式]按已排序手牌找最小合法單張，多張頂牌時選擇 Pass
        /// </summary>
        private List<PlayingCard> FindComputerSingle(int player)
        {
            foreach (PlayingCard card in _hands[player].Cards)
            {
                var candidate = new List<PlayingCard> { card };
                if (_match.CanPlay(player, candidate, out _)) return candidate;
            }
            return null;
        }

        /// <summary>
        /// 提交前保存索引
        /// </summary>
        private List<int> CaptureIndices(int player, IReadOnlyList<PlayingCard> cards)
        {
            var indices = new List<int>();
            foreach (PlayingCard card in cards)
            {
                int index = _hands[player].IndexOf(card);
                indices.Add(index);
            }
            return indices;
        }

        /// <summary>
        /// 提交成功更新View
        /// </summary>
        private void ShowCommittedPlay(int player, IReadOnlyList<int> indices,
            BigTwoPlay play)
        {
            _tableLayout.MoveAllTo(_discardRoot);
            foreach (PlayingCard card in play.Cards) card.ShowUp();
            _playerLayouts[player].MoveCardsTo(_tableLayout, indices);
            _tableLayout.ReBindCards(play.Cards);
            _selection.Clear();
            RefreshSelectionView();
        }

        /// <summary>
        /// 勝負確定就停止；否則啟動下一家行動
        /// </summary>
        private void ContinueGame()
        {
            if (_match.IsComplete)
            {
                ShowTurn();
                RefreshControls();
                return;
            }
            RunFlow(false);
        }

        /// <summary>
        /// SHOW出第一位出完的贏家
        /// </summary>
        private void ShowTurn()
        {
            UpdateStatusUI(_match.IsComplete ?
                $"Player {_match.WinnerIndex + 1} Wins! Press Start to replay." :
                $"Player {_match.Round.CurrentPlayerIndex + 1} Play Cards.");
        }

        /// <summary>
        /// End.終結必然要執行
        /// </summary>
        private void CancelFlow()
        {
            //不管任何形式的被結束、關閉：用 TASK 的 Token 通知任務已結束
            //避免卡死成為殭屍行程
            _flowCancellation?.Cancel();
        }
        /// <summary>
        /// 更新狀態文字UI
        /// </summary>
        /// <param name="msg">訊息</param>
        private void UpdateStatusUI(string msg)
        {
            _statusLabel.text = msg;
        }

        /// <summary>
        /// 回收 View 並且整理(清除)四組玩家手牌資料
        /// </summary>
        private void ReleaseCrads()
        {
            //遊戲可操作狀態鎖定
            _ready = false;
            //卡牌選取狀態清除 & 刷新
            _selection.Clear();
            RefreshSelectionView();
            //荷官回收卡牌(資料)
            _dealer.CollectAll();
            foreach (BigTwoHand hand in _hands)
            {//清除4組手牌資料(視覺)
                hand.Clear();
            }
        }
        /// <summary>
        /// Selection手牌狀態重畫(刷新)
        /// </summary>
        private void RefreshSelectionView()
        {
            if (_playerLayouts != null &&
                _playerLayouts.Length > 0 &&
                _playerLayouts[0] != null)
            {//刷新選取的手牌視覺
                List<int> indices = CaptureIndices(0, _selection.Cards);
                _playerLayouts[0].SelectionToggle(indices);
            }
        }
        #endregion 私有方法
    }
}

