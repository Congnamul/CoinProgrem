using System.Collections.Generic;

[System.Serializable]
public class CoinData
{
    public string name; // �̸�

    public int nowCost; //���� �ְ�
    public int lastUpdate; //�ֱ� ������

    public int bringCount; //���� �ֽ� ����
    public long totalCost; //���� �ֽ� ����
    public long useMoney; //���Ծ�

    public int[] changeUp; //��°� ����, �ּ�
    public int[] changeDown; //���Ⱚ �ִ�, �ּ�

    public float persent; //��� �϶� �ۼ�Ʈ
    public float getPersent; //���ͷ� �ۼ�Ʈ
    public int Chance; //��� Ȯ��









}
