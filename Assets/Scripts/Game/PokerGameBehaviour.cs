using UnityEngine;

namespace PokerGame.Game
{
    /// <summary>
    /// [基本欄位/功能邏輯]撲克遊戲的抽象層
    /// </summary>
    public abstract class PokerGameBehaviour : MonoBehaviour, IPokerGameMode
    {
        #region 公開屬性
        public abstract string GameName { get; }
        #endregion 公開屬性

        #region 欄位
        [SerializeField]
        protected Dealer _dealer;
        protected TableSession _session;
        #endregion 欄位

        #region 生命週期
        private void Awake()
        {
            //指定/建立 TableSession
            _session = TableSession.Instance;
        }
        #endregion 生命週期

        #region 公開方法
        public abstract void EnterMode();

        public abstract void ExitMode();
        #endregion 公開方法
    }
}

