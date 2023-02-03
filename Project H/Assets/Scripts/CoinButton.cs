using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinButton : MonoBehaviour
{

    public int coinID = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform coinTag = GameObject.FindGameObjectWithTag("CoinTextTag").transform;
        for(int i = 0; i < coinTag.childCount; i++)
        {
            if( coinTag.GetChild(i).gameObject == transform.gameObject)
            {
                coinID = i;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        GameManager.Instance.nowCoinID = coinID;
        GameManager.Instance.countInput.text = "";
    }




}
