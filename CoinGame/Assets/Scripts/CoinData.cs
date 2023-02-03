using System.Collections.Generic;

[System.Serializable]
public class CoinData
{
    public string name; // 이름

    public int nowCost; //현재 주가
    public int lastUpdate; //최근 변동값

    public int bringCount; //보유 주식 개수
    public long totalCost; //보유 주식 총합
    public long useMoney; //구입액

    public int[] changeUp; //상승값 쵀대, 최소
    public int[] changeDown; //하향값 최대, 최소

    public float persent; //상승 하락 퍼센트
    public float getPersent; //수익률 퍼센트
    public int Chance; //상승 확률









}
