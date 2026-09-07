using PokerGame.View;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

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
            //各發13張到4家

        }
        #endregion 私有方法
    }
}

