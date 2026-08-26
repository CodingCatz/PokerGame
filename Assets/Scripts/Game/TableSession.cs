using PokerGame.Betting;
using UnityEngine;

namespace PokerGame.Game
{
    /// <summary>
    /// [單例模式]跨遊戲共用的籌碼管理與下注系統
    /// </summary>
    public class TableSession : MonoBehaviour
    {
        #region 靜態存取參數
        /// <summary>
        /// [靜態]Table單一物件實例(存取接口)
        /// </summary>
        public static TableSession Instance 
        {
            get
            {
                if (_instance == null)
                {//實體不存在：
                 //立刻建立GameObject載體.掛上TableSession腳本，並存放於唯一實體
                    _instance = new GameObject("TableSession").AddComponent<TableSession>();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 物件唯一實體
        /// </summary>
        private static TableSession _instance;
        #endregion 靜態存取參數

        #region 公開屬性
        public BettingSystem Betting { get; private set; }
        #endregion 公開屬性

        #region 生命週期
        private void Awake()
        {
            //建立下注系統與分配錢包
            Betting = new BettingSystem(new ChipWallet(1000));
            //為了能跨場景留存(UNITY提供的功能)：不銷毀(此遊戲物件)
            DontDestroyOnLoad(gameObject);
        }
        #endregion 生命週期
    }
}
