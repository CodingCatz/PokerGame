using UnityEngine;//using 使用XXXX命名空間，同等於資料夾的概念
using PokerGame.Core;
using PokerGame.View;

//繼承於UNITY核心腳本：MonoBehaviour，能在前台運作
public class CradDemo : MonoBehaviour
{
    //宣告類型 名稱 = 新建 實體();
    public PlayingCard testCard = new PlayingCard(Suit.Spades, Rank.King);
    public CardView cardView;

    //按下PLAY後執行一次，初始化
    void Start()
    {
        //利用除錯訊息視窗印出指定內容
        Debug.Log(testCard.Info());
        
    }

    //遊戲每一FPS執行一次，偵測操作/刷新內容
    void Update()
    {
        //視覺顯示.綁定(卡牌資料)
        cardView.Bind(testCard);
    }
}
