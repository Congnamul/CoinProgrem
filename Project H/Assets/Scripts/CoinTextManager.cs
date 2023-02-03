using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTextManager : MonoBehaviour
{
    Coin coin;

    //public List<Text> coinText = new List<Text>();
    public GameObject coinTextPrefebs;
    public Transform coinTextParent;

    public GameObject[] coinText;

    //Player Monney
    public Text allMoneyText;
    public Text nowMoneyText;
    public Text nowMoneyTextMain;

    //주식 창 자세한 정보
    public Text[] CenterCoinText;

    // Start is called before the first frame update
    void Start()
    {
        coin = GetComponent<Coin>();

        //Coin스크립트의 coindata개수만큼 텍스트 칸 생성
        coinText = new GameObject[coin.coinDatas.Length];
        for (int i = 0; i < coin.coinDatas.Length; i++)
        {
            coinText[i] = Instantiate(coinTextPrefebs, coinTextParent);
        }

        CoinTextUpdate();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoney();
    }

    //숫자 , 변환
    string TransNum(long num)
    {
        return string.Format("{0:#,0}", num);
    }


    //플레이어 돈 텍스트표시
    public void PlayerMoney()
    {
        //allMoneyText.text = GameManager.Instance.PlayerNowMoney + "원";
        nowMoneyText.text = "보유 금액 " + TransNum(GameManager.Instance.PlayerNowMoney) + "원";
        nowMoneyTextMain.text = "보유 금액 \n" + TransNum(GameManager.Instance.PlayerNowMoney) + "원";

        allMoneyText.text = "총 자산 " + TransNum(GameManager.Instance.PlayerAllMoney) + "원";

    }

    public void CoinTextUpdate()
    {

        for (int i = 0; i < coin.coinDatas.Length; i++)
        {
            Text Ctext;
            
            Ctext = coinText[i].transform.GetChild(0).GetComponent<Text>();
            Ctext.text = coin.coinDatas[i].name;

            Ctext = coinText[i].transform.GetChild(1).GetComponent<Text>();
            Ctext.text = coin.coinDatas[i].nowCost + "";
            
            Ctext = coinText[i].transform.GetChild(2).GetComponent<Text>();
            if (coin.coinDatas[i].persent >= 0)
            {
                Ctext.text = "+" + string.Format("{0:N2}", coin.coinDatas[i].persent) + "%";
                Ctext.color = Color.red;
            }
            else
            {
                Ctext.text = string.Format("{0:N2}", coin.coinDatas[i].persent) + "%";
                Ctext.color = Color.blue;
            }

            if(coin.coinDatas[i].useMoney != 0)
            {
                float a = ((coin.coinDatas[i].bringCount * coin.coinDatas[i].nowCost) - coin.coinDatas[i].useMoney);
                float b = coin.coinDatas[i].useMoney;
                coin.coinDatas[i].getPersent = Mathf.Round(a / b * 100 * 100) * 0.01f;
            }
                


        }
    }

    public void CenterCoinUpdate(int id)
    {
        CenterCoinText[0].text = coin.coinDatas[id].name;
        CenterCoinText[1].text = "현재가 : " + coin.coinDatas[id].nowCost + "원";
        CenterCoinText[2].text = "변동값 : " + coin.coinDatas[id].lastUpdate + "원";
        CenterCoinText[3].text = "총주식 : " + coin.coinDatas[id].bringCount + "주";
        CenterCoinText[4].text = "총자산 : " + coin.coinDatas[id].bringCount * coin.coinDatas[id].nowCost + "원";
        CenterCoinText[5].text = "구매가격 : " + coin.coinDatas[id].useMoney + "원";
        CenterCoinText[6].text = "평가손익 : " + ((coin.coinDatas[id].bringCount * coin.coinDatas[id].nowCost) - coin.coinDatas[id].useMoney) + "원";
        CenterCoinText[7].text = "수익률 : " + string.Format("{0:N2}", coin.coinDatas[id].getPersent) + "%";



    }


}
