using PokerGame.View;
using UnityEngine;
using TMPro;

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
        
        [SerializeField]
        private CardHandLayout[] _playerLayouts;
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
            
        }
        #endregion 生命週期

        #region 公開方法
        public override void EnterMode()
        {
            //開始發牌
        }

        public override void ExitMode()
        {
            //荷官整理桌面
        }

        #endregion 公開方法

    }
}

