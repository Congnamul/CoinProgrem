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
        talkData.Add(100, new string[] { "하루를 마무리 하시겠습니까?" }); //침대
        talkData.Add(200, new string[] { "집 밖으로 나가시겠습니까?" }); //집문
        talkData.Add(500, new string[] { "컴퓨터 전원을 작동시킵니다." }); //컴퓨터

        talkData.Add(1000, new string[] { "집으로 들어가시겠습니까?" }); //집 안
        talkData.Add(2000, new string[] { "뒷골목으로 진입하시겠습니까?" }); //뒷골목
        talkData.Add(3000, new string[] { "회사에 출근 하시겠습나까?" }); //회사

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

}
