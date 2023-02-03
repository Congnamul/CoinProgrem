using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //�̱��� ����
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

    //����ȭ�� �ֽ� �з� ID
    public int nowCoinID = 0;

    //���� ����
    public int countBuySell;
    public InputField countInput;

    //��
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
        //�� ��� ���ΰ�ħ
        PlayerMoneySelect();

        //��ǻ�� ȭ�� ���ΰ�ħ
        CoinTM.CenterCoinUpdate(nowCoinID);

        //���� ���ΰ�ħ
        if (Input.GetKeyDown("space"))
        {
            coin.UpdateCoin();
        }


    }

    //�÷��̾� �ڻ� ���
    public void PlayerMoneySelect()
    {
        PlayerAllMoney = PlayerNowMoney;
        
        for (int i = 0; i < coin.coinDatas.Length; i++)
        {
            coin.coinDatas[i].totalCost = coin.coinDatas[i].bringCount * coin.coinDatas[i].nowCost;
            PlayerAllMoney += coin.coinDatas[i].totalCost;
        }
    }

    //����, �Ǹ� ���� ����
    public void ExchangeCount()
    {
        countBuySell = int.Parse(countInput.text);
    }

    //�ֽ� ����
    public void BuyCoin()
    {
        if(PlayerNowMoney - (coin.coinDatas[nowCoinID].nowCost * countBuySell) >= 0)
        {
            //�� ����
            PlayerNowMoney -= (coin.coinDatas[nowCoinID].nowCost * countBuySell);
            
            //�����ֽ� ����
            coin.coinDatas[nowCoinID].bringCount += countBuySell;
            
            //���Ծ� ����
            coin.coinDatas[nowCoinID].useMoney += (coin.coinDatas[nowCoinID].nowCost * countBuySell);

            countInput.text = "";

            CoinTM.CoinTextUpdate();
        }
    }

    //�ֽ� �Ǹ�
    public void SellCoin()
    {
        if( coin.coinDatas[nowCoinID].bringCount > 0)
        {
            //�� ����
            PlayerNowMoney += (coin.coinDatas[nowCoinID].nowCost * coin.coinDatas[nowCoinID].bringCount);

            //�����ֽ� ����
            coin.coinDatas[nowCoinID].bringCount = 0;

            //���Ծ� ����
            coin.coinDatas[nowCoinID].useMoney = 0;

            CoinTM.CoinTextUpdate();
        }

    }




}
