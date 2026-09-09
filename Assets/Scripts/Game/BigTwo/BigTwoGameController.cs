using PokerGame.View;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using PokerGame.Core;
using System.Threading.Tasks;

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
        #endregion UI元件

        #region 欄位
        /// <summary>
        /// 遊戲的延遲時間(速度)，對外公開可調的接口
        /// </summary>
        [Range(0.2f, 2f)]//轉成有範圍的拉桿
        [SerializeField]
        private float _delayTime = 1f;
        /// <summary>
        /// 視覺排版
        /// </summary>
        [SerializeField]
        private CardHandLayout[] _playerLayouts;
        /// <summary>
        /// 手牌資料
        /// </summary>
        private readonly List<BigTwoHand> _hands = new List<BigTwoHand>();
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
        void Start()
        {
            EnterMode();
            StartGame();
        }
        #endregion 生命週期

        #region 公開方法
        public override void EnterMode()
        {
            //建立手牌資料容器 * 4
            EnsureHands();
            //開始發牌(訊息)
            _statusLabel.text = "Start Deal Four Hands.";
        }

        public override void ExitMode()
        {
            //荷官整理桌面
            _dealer.CollectAll();
            ClearHands();
        }


        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 確認(建立)四組玩家手牌容器
        /// </summary>
        private void EnsureHands()
        {
            for (int i = 0; i < _playerLayouts.Length; i++)
            {
                _hands.Add(new BigTwoHand());
            }
        }
        /// <summary>
        /// 整理(清除)四組玩家手牌容器
        /// </summary>
        private void ClearHands()
        {
            for (int i = 0; i < _hands.Count; i++)
            {//清除4組手牌資料(視覺)
                _hands[i].Clear();
            }
        }
        /// <summary>
        /// 開始牌局
        /// </summary>
        private void StartGame()
        {
            _dealer.BeginRound();
            ClearHands();
            StartDeal(GameSpeed);
        }

        /// <summary>
        /// [非同步行程]開局的發牌行程
        /// </summary>
        /// <param name="msec">遊戲速度</param>
        private async Task StartDeal(int msec)
        {
            //各發13張到4家(※有可能不足4，以實際的hands數量為主)
            for (int round = 0; round < 13; round++)
            {
                for (int playerIndex = 0; playerIndex < _hands.Count; playerIndex++)
                {
                    //視覺實體+資料分配：各玩家
                    PlayingCard card = _dealer.DealTo(_playerLayouts[playerIndex].Root);
                    _hands[playerIndex].Add(card);
                    //狀態更新
                    _playerLayouts[playerIndex].Refresh();
                    await Task.Delay(msec);//等待：延遲任務
                }
            }
        }
        #endregion 私有方法
    }
}

