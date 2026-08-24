using System;
using UnityEngine;

namespace PokerGame.Betting
{
    /// <summary>
    /// 保存玩家籌碼餘額
    /// </summary>
    public class ChipWallet
    {
        #region 公開屬性
        /// <summary>
        /// 取得籌碼餘額(只允許透過功能做內部設定)
        /// </summary>
        public int Balance { get; private set; }
        #endregion 公開屬性

        #region 建構式
        /// <summary>
        /// 開啟籌碼錢包(初始化儲值金額)
        /// </summary>
        /// <param name="initialBalance">儲值金額</param>
        public ChipWallet(int initialBalance)
        {
            //基本的數據防護：取最小值的防呆措施
            Balance = Math.Max(0, initialBalance);
        }
        #endregion 建構式

        #region 公開方法
        /// <summary>
        /// 嘗試進行提領行為
        /// </summary>
        /// <param name="amount">要提領的金額</param>
        /// <returns>是否成功</returns>
        public bool TryWithdraw(int amount)
        {
            //提領負值 or 提領金額大於結餘：回傳失敗，並阻擋後續程式碼
            if (amount <= 0 || amount > Balance) return false;
            //沒被阻擋才合法扣除
            Balance -= amount;
            return true;//回傳成功
        }
        /// <summary>
        /// 存入非負數值的金額
        /// </summary>
        /// <param name="amount">>要存入的金額</param>
        public void Deposit(int amount)
        {
            //結餘加等於取最小值的金額(防呆)
            Balance += Math.Max(0, amount);
        }

        #endregion 公開方法
    }
}
