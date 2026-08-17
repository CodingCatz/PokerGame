using UnityEngine;
using TMPro;
using PokerGame.Core;

namespace PokerGame.View
{
    public class CardView : MonoBehaviour
    {
        #region UI元件
        [SerializeField]//強制將私有欄位顯示在編輯器
        private TMP_Text _suitLabel;
        [SerializeField]
        private TMP_Text _rankLabel;
        #endregion UI元件

        #region 公開方法
        public void Bind(PlayingCard card)
        {
            _suitLabel.text = card.Suit.ToString();
            _rankLabel.text = card.Rank.ToString();
        }
        #endregion 公開方法

        #region 私有方法
        #endregion 私有方法
    }
}
