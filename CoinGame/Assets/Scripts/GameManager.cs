using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //싱글톤 패턴
    private static GameManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }
    //-----
    public GameObject coinManager;
    Coin coin;
    CoinTextManager CoinTM;
    public Camera mainCamera;
    public bool isCamMove;

    //센터화면 주식 분류 ID
    public int nowCoinID = 0;

    //구매 개수
    public int countBuySell;
    public InputField countInput;

    //돈
    public long PlayerNowMoney;
    public long PlayerAllMoney;
    long lastMoney;


    // Start is called before the first frame update
    void Start()
    {
        CoinTM = coinManager.GetComponent<CoinTextManager>();
        coin = coinManager.GetComponent<Coin>();


    }

    // Update is called once per frame
    void Update()
    {
        //돈 계산 새로고침
        PlayerMoneySelect();

        //컴퓨터 화면 새로고침
        CoinTM.CenterCoinUpdate(nowCoinID);

        //코인 새로고침
        if (Input.GetKeyDown("space"))
        {
            coin.UpdateCoin();
        }


    }

    //플레이어 자산 계산
    public void PlayerMoneySelect()
    {
        PlayerAllMoney = PlayerNowMoney;
        
        for (int i = 0; i < coin.coinDatas.Length; i++)
        {
            coin.coinDatas[i].totalCost = coin.coinDatas[i].bringCount * coin.coinDatas[i].nowCost;
            PlayerAllMoney += coin.coinDatas[i].totalCost;
        }
    }

    //구매, 판매 개수 설정
    public void ExchangeCount()
    {
        countBuySell = int.Parse(countInput.text);
    }

    //주식 구매
    public void BuyCoin()
    {
        if(PlayerNowMoney - (coin.coinDatas[nowCoinID].nowCost * countBuySell) >= 0)
        {
            //돈 차감
            PlayerNowMoney -= (coin.coinDatas[nowCoinID].nowCost * countBuySell);
            
            //보유주식 변경
            coin.coinDatas[nowCoinID].bringCount += countBuySell;
            
            //구입액 변경
            coin.coinDatas[nowCoinID].useMoney += (coin.coinDatas[nowCoinID].nowCost * countBuySell);

            countInput.text = "";

            CoinTM.CoinTextUpdate();
        }
    }

    //주식 판매
    public void SellCoin()
    {
        if( coin.coinDatas[nowCoinID].bringCount > 0)
        {
            //돈 증가
            PlayerNowMoney += (coin.coinDatas[nowCoinID].nowCost * coin.coinDatas[nowCoinID].bringCount);

            //보유주식 변경
            coin.coinDatas[nowCoinID].bringCount = 0;

            //구입액 변경
            coin.coinDatas[nowCoinID].useMoney = 0;

            CoinTM.CoinTextUpdate();
        }

    }




}
