using UnityEngine;

namespace PokerGame.Game
{
    /// <summary>
    /// 定義撲克牌遊戲類型該提供的基本資訊、操作
    /// </summary>
    public interface IPokerGameMode
    {
        #region 公開屬性
        /// <summary>
        /// 取得在遊戲大廳顯示的遊戲模式名稱
        /// </summary>
        string GameName { get; }
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 進入模式並初始化操作狀態
        /// </summary>
        void EnterMode();
        /// <summary>
        /// 離開模式並釋放、清除佔用資源
        /// </summary>
        void ExitMode();
        #endregion 公開方法
    }
}

