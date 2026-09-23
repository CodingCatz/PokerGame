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
        #endregion UI元件

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
        #endregion 公開屬性

        #region 生命週期
        //建立手牌資料容器 * 4
        private void Awake() => EnsureHands();
        //程式腳本被關閉時觸發(單純的停止遊戲資料的回收機制)
        private void OnDisable() => ExitMode();
        //場景被卸載(物件被銷毀)，徹底停止一些背景任務(執行序)
        private void OnDestroy() => CancelFlow();
        #endregion 生命週期

        #region 公開方法
        /// <summary>
        /// [UI按鈕]開始牌局
        /// </summary>
        public void StartGame()
        {
            EnterMode();
            //產生一個 TASK 的辨識碼(行程的ID)
            _flowCancellation = new CancellationTokenSource();
            _dealer.BeginRound();
            ReleaseCrads();
            //外部任務捨棄(因為沒有要排隊)
            _ = StartDealAsync(GameSpeed, _flowCancellation.Token);
        }

        /// <summary>
        /// 2.按下開始後顯示發牌訊息 & 更新桌面視覺狀態
        /// </summary>
        public override void EnterMode()
        {
            //開始發牌(訊息)
            UpdateStatusUI("Start Deal Four Hands.");
            _startBtn?.gameObject.SetActive(false);
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

        /// <summary>
        /// [UI按鈕]出牌(選取的)
        /// </summary>
        public void PlaySelectedCards()
        {
            _playerLayouts[0].MoveCardsTo(_tableLayout);
        }
        /// <summary>
        /// [被委派]每個CardView的點擊觸發
        /// </summary>
        /// <param name="view"></param>
        public void CardViewClick(CardView view)
        {
            if (!_match.IsHumanRound) return;
            //手牌 Layout 被選中的視覺對應序號紀錄
            int index = _playerLayouts[0].SelectionToggle(view);
            if (index < 0) return;
            //執行選取紀錄
            _selection.Toggle(_hands[0].Cards[index]);
            //送驗牌員：紀錄是否成配對成組
            bool canPlay = _match.CanPlay(0, _selection.Cards, out BigTwoPlay play);
            //出牌鈕狀態更新
            _playBtn.gameObject.SetActive(canPlay);
            //印出牌型
            if (play != null) _playLabel.text = play.Type.ToString();
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 1.顯示開始遊戲按鈕和資訊 & 確認(建立)四組玩家手牌容器
        /// </summary>
        private void EnsureHands()
        {
            UpdateStatusUI("Ready to Play ?");
            _startBtn?.gameObject.SetActive(true);
            for (int i = 0; i < _playerLayouts.Length; i++)
            {
                _hands.Add(new BigTwoHand());
            }
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
                //_playerLayouts[0].
            }
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
        #endregion 私有方法
    }
}

