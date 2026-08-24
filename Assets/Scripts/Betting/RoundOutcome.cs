using UnityEngine;

namespace PokerGame.Betting
{
    /// <summary>
    /// [列舉]每一回合相對於玩家來說的結果
    /// </summary>

    public enum RoundOutcome
    {
        /// <summary>
        /// 玩家勝
        /// </summary>
        Win,
        /// <summary>
        /// 玩家敗
        /// </summary>
        Lose,
        /// <summary>
        /// 平手退注
        /// </summary>
        Push
    }
}
