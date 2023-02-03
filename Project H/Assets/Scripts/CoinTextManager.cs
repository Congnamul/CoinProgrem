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

    //�ֽ� â �ڼ��� ����
    public Text[] CenterCoinText;

    // Start is called before the first frame update
    void Start()
    {
        coin = GetComponent<Coin>();

        //Coin��ũ��Ʈ�� coindata������ŭ �ؽ�Ʈ ĭ ����
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

    //���� , ��ȯ
    string TransNum(long num)
    {
        return string.Format("{0:#,0}", num);
    }


    //�÷��̾� �� �ؽ�Ʈǥ��
    public void PlayerMoney()
    {
        //allMoneyText.text = GameManager.Instance.PlayerNowMoney + "��";
        nowMoneyText.text = "���� �ݾ� " + TransNum(GameManager.Instance.PlayerNowMoney) + "��";
        nowMoneyTextMain.text = "���� �ݾ� \n" + TransNum(GameManager.Instance.PlayerNowMoney) + "��";

        allMoneyText.text = "�� �ڻ� " + TransNum(GameManager.Instance.PlayerAllMoney) + "��";

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
        CenterCoinText[1].text = "���簡 : " + coin.coinDatas[id].nowCost + "��";
        CenterCoinText[2].text = "������ : " + coin.coinDatas[id].lastUpdate + "��";
        CenterCoinText[3].text = "���ֽ� : " + coin.coinDatas[id].bringCount + "��";
        CenterCoinText[4].text = "���ڻ� : " + coin.coinDatas[id].bringCount * coin.coinDatas[id].nowCost + "��";
        CenterCoinText[5].text = "���Ű��� : " + coin.coinDatas[id].useMoney + "��";
        CenterCoinText[6].text = "�򰡼��� : " + ((coin.coinDatas[id].bringCount * coin.coinDatas[id].nowCost) - coin.coinDatas[id].useMoney) + "��";
        CenterCoinText[7].text = "���ͷ� : " + string.Format("{0:N2}", coin.coinDatas[id].getPersent) + "%";



    }


}
