using System;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

namespace PokerGame.Game.BigTwo
{
    /// <summary>
    /// 保存4家輪替、桌面頂牌、Pass狀態
    /// </summary>
    public class BigTwoRound
    {
        #region 常數
        /// <summary>
        /// 常態性玩家總數量
        /// </summary>
        private const int PlayerCount = 4;
        #endregion 常數

        #region 私有欄位
        private readonly BigTwoPlayComparer _comparer 
            = new BigTwoPlayComparer();
        /// <summary>
        /// PASS的累計數量
        /// </summary>
        private int _passes;
        #endregion 私有欄位

        #region 公開屬性
        /// <summary>
        /// 正在行動中玩家的序列號
        /// </summary>
        public int CurrentPlayerIndex { get; private set; }
        /// <summary>
        /// 最後一次成功出牌玩家的序列號
        /// </summary>
        public int LeaderIndex { get; private set; }
        /// <summary>
        /// 目前的頂牌組合
        /// </summary>
        public BigTwoPlay TopPlay { get; private set; }
        /// <summary>
        /// 目前回合狀態
        /// </summary>
        public BigTwoRoundState State { get; private set; }
        #endregion 公開屬性

        #region 公開方法
        /// <summary>
        /// 空桌：指定起始出牌玩家
        /// </summary>
        /// <param name="playerIndex"></param>
        public void StartPlayer(int playerIndex)
        {
            TopPlay = null;
            CurrentPlayerIndex = playerIndex;
            LeaderIndex = playerIndex;//以防萬一
            State = BigTwoRoundState.AwaitingLead;
            _passes = 0;
        }

        /// <summary>
        /// 是否可出牌：當前輪到的操作者 & 牌型組合成立 & 要蓋(大)過桌面上的
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="play"></param>
        /// <returns></returns>
        public bool CanPlay(int playerIndex, BigTwoPlay play)
        {
            return playerIndex == CurrentPlayerIndex && 
                play != null && _comparer.CanBeat(play, TopPlay);
        }

        /// <summary>
        /// 合法出牌才能蓋過領先者牌型，交棒給下一位
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="play"></param>
        /// <returns></returns>
        public bool TryPlay(int playerIndex, BigTwoPlay play)
        {
            if (!CanPlay(playerIndex, play)) return false;
            TopPlay = play;//記錄頂牌
            LeaderIndex = playerIndex;//紀錄出牌者
            State = BigTwoRoundState.AwaitingResponse;//狀態設為可被回應(跟牌)
           
            return true;
        }
        /// <summary>
        /// 有頂牌才允許PASS
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public bool TryPass(int playerIndex)
        {
            if (playerIndex != CurrentPlayerIndex || 
                State != BigTwoRoundState.AwaitingResponse)
                return false;
            //按下PASS後的邏輯
            _passes++;//PASS人數+1
            if (_passes == PlayerCount - 1)
            {//放棄人數已滿：總玩家數-1
                //清桌重置
                TopPlay = null;//頂牌重置
                //可出牌者交給最後成功出牌者(生存者)
                CurrentPlayerIndex = LeaderIndex;
                //狀態切回至首出
                State = BigTwoRoundState.AwaitingLead;
                //PASS人數歸0重計
                _passes = 0;
            }
            else
            {//放棄人數未滿：跳至下一位合法操作者
                CurrentPlayerIndex = NextPlayer(playerIndex);
            }

            return true;
        }
        #endregion 公開方法

        #region 私有方法
        /// <summary>
        /// 取餘數獲取玩家序號的循環
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        private int NextPlayer(int playerIndex)
        {//取餘數：目的是為了能循環到第一位
            return playerIndex + 1 % PlayerCount;
        }
        #endregion 私有方法
    }
}