using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using System;
using LitJson;

public class Coin : MonoBehaviour
{
    public CoinData[] coinDatas;
    [SerializeField] TextAsset csvAasset;
    CoinTextManager coinTM;


    private void Awake()
    {
        CoinLoad();
    }

    private void Start()
    {
        coinTM = GetComponent<CoinTextManager>();
    }

    private void Update()
    {
        //coinDatas.Remove
        if (Input.GetMouseButtonDown(2))
        {
            UpdateCoin();
        }
    }

    //���� �ε�
    [ContextMenu("DotestLoad")]
    void CoinLoad()
    {
        coinDatas = CsvUtility.CsvToArray<CoinData>(csvAasset.text);
    }

    //���� ����
    string filePath => Path.Combine(Application.dataPath, "Resources/CoinData.csv");
    [ContextMenu("DoTestSave")]
    void CoinSave()
    {
        string csv = CsvUtility.ArrayToCsv(coinDatas);
        Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        StreamWriter outStream = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        outStream.Write(csv);
        outStream.Close();
    }



    //�ֽ� �� ������Ʈ
    [ContextMenu("CoinUpdate")]
    public void UpdateCoin()
    {
        //�ֽ� ��ü�� �ѹ��� ����
        for(int i = 0; i < coinDatas.Length; i++)
        {

            //���Ȯ�� ���� �ȿ� �� �� ��� �ƴ� �� ����
            int upchance = Random.Range(0, 100);
            coinDatas[i].lastUpdate = (upchance <= coinDatas[i].Chance) ?
                Random.Range(coinDatas[i].changeUp[0], coinDatas[i].changeUp[1]) :
                -Random.Range(coinDatas[i].changeDown[0], coinDatas[i].changeDown[1]);

            //������� ����
            coinDatas[i].lastUpdate = DeleteTen(coinDatas[i].lastUpdate);

            //��°� �ۼ�Ʈ�� ǥ��
            coinDatas[i].persent = Persent( coinDatas[i].nowCost, coinDatas[i].lastUpdate);

            //����� �ְ��� 0���ϰ� �ȴٸ� �ٽ÷���
            if ((coinDatas[i].nowCost + coinDatas[i].lastUpdate) < 0)
            {
                i -= 1;
                continue;
            }
            else 
            {
                coinDatas[i].nowCost += coinDatas[i].lastUpdate;
            }

            //�ڽ��� ������ �ִ� �ֽ� ����
            if(coinDatas[i].bringCount != 0)
                coinDatas[i].totalCost = coinDatas[i].bringCount * coinDatas[i].nowCost;
            
            
        }

        //�ؽ�Ʈ ������Ʈ
        coinTM.CoinTextUpdate();

    }


    //�����ڸ����� 0���� ��ȯ
    int DeleteTen(int value)
    {
        int val = value;
        val = val % 100;
        value -= val;
        return value;
    }
    
    float Persent(float now, float update)
    {
        float per = (update / now) * 100;
        per = Mathf.Floor(per * 100) * 0.01f;
        return per;
    }
}
