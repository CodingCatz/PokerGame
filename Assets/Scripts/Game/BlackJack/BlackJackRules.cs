using PokerGame.Betting;
using UnityEngine;

namespace PokerGame.Game.BlackJack
{
    /// <summary>
    /// [裁判]21點的遊戲規則，依照達成難易排序
    /// </summary>
    public class BlackJackRules
    {
        #region 公開方法
        /// <summary>
        /// 雙方全部勝負組合與賠率
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dealer"></param>
        /// <returns></returns>
        public RoundResult Resolve(BlackJackHand player, BlackJackHand dealer)
        {
            if (player.Points > 21)//玩家爆牌：籌碼歸0
                return new RoundResult(RoundOutcome.Lose, 0f, "Player Busts.");
            if (player.IsBlackJack && dealer.IsBlackJack)//雙黑傑克：籌碼退還
                return new RoundResult(RoundOutcome.Push, 1f, "Both BlackJack.");
            if (player.IsBlackJack)//玩家21點勝：多退1.5倍
                return new RoundResult(RoundOutcome.Win, 2.5f, "BlackJack!!");
            if (dealer.IsBlackJack)//莊家21點勝：多籌碼歸0
                return new RoundResult(RoundOutcome.Lose, 0f, "BlackJack!!");
            if (player.IsPass5)//玩家過5關：退2倍
                return new RoundResult(RoundOutcome.Win, 3f, "Pass Five!!");
            if (dealer.Points > 21 || player.Points > dealer.Points)//莊家爆 或 點數勝
                return new RoundResult(RoundOutcome.Win, 2f, "Win!!");
            if (player.Points < dealer.Points)//點數輸
                return new RoundResult(RoundOutcome.Lose, 0f, "Lose!!");

            return new RoundResult(RoundOutcome.Push, 1f, "");
        }
        #endregion 公開方法
    }
}

