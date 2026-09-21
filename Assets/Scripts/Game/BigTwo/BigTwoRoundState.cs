using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 桌面處於新首出或跟牌回應狀態
    /// </summary>
    public enum BigTwoRoundState
    {
        /// <summary>
        /// 桌面為空時：不限牌型
        /// </summary>
        AwaitingLead,
        /// <summary>
        /// 桌面有組合：牌型要壓過(否則只能PASS)
        /// </summary>
        AwaitingResponse
    }
}