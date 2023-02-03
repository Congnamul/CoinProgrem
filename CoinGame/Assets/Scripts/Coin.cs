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

    //파일 로드
    [ContextMenu("DotestLoad")]
    void CoinLoad()
    {
        coinDatas = CsvUtility.CsvToArray<CoinData>(csvAasset.text);
    }

    //파일 저장
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



    //주식 값 업데이트
    [ContextMenu("CoinUpdate")]
    public void UpdateCoin()
    {
        //주식 전체를 한번에 루프
        for(int i = 0; i < coinDatas.Length; i++)
        {

            //상승확률 범위 안에 들어갈 시 상승 아닐 시 하향
            int upchance = Random.Range(0, 100);
            coinDatas[i].lastUpdate = (upchance <= coinDatas[i].Chance) ?
                Random.Range(coinDatas[i].changeUp[0], coinDatas[i].changeUp[1]) :
                -Random.Range(coinDatas[i].changeDown[0], coinDatas[i].changeDown[1]);

            //백단위로 변등
            coinDatas[i].lastUpdate = DeleteTen(coinDatas[i].lastUpdate);

            //상승값 퍼센트로 표시
            coinDatas[i].persent = Persent( coinDatas[i].nowCost, coinDatas[i].lastUpdate);

            //하향시 주가가 0이하가 된다면 다시루프
            if ((coinDatas[i].nowCost + coinDatas[i].lastUpdate) < 0)
            {
                i -= 1;
                continue;
            }
            else 
            {
                coinDatas[i].nowCost += coinDatas[i].lastUpdate;
            }

            //자신이 가지고 있는 주식 정산
            if(coinDatas[i].bringCount != 0)
                coinDatas[i].totalCost = coinDatas[i].bringCount * coinDatas[i].nowCost;
            
            
        }

        //텍스트 업데이트
        coinTM.CoinTextUpdate();

    }


    //십의자리부터 0으로 변환
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
