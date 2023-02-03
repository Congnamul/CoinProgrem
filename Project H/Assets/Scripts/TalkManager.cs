using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{

    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(100, new string[] { "�Ϸ縦 ������ �Ͻðڽ��ϱ�?" }); //ħ��
        talkData.Add(200, new string[] { "�� ������ �����ðڽ��ϱ�?" }); //����
        talkData.Add(500, new string[] { "��ǻ�� ������ �۵���ŵ�ϴ�." }); //��ǻ��

        talkData.Add(1000, new string[] { "������ ���ðڽ��ϱ�?" }); //�� ��
        talkData.Add(2000, new string[] { "�ް������ �����Ͻðڽ��ϱ�?" }); //�ް��
        talkData.Add(3000, new string[] { "ȸ�翡 ��� �Ͻðڽ�����?" }); //ȸ��

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

}
