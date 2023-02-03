using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csvReadMan : MonoBehaviour
{
	public int CoinCost = 0;

	//»ý·«
	void Start()
	{

		List<Dictionary<string, object>> datas = csvReader.Read("CoinData");

		for (var i = 0; i < datas.Count; i++)
		{
			//print("nowCost " + datas[i]["nowCost"] + " ");
		}
		CoinCost = (int)datas[0]["nowCost"];
		Debug.Log(CoinCost);
		datas[0]["nowCost"] = 10;
		Debug.Log((int)datas[0]["nowCost"]);

	}



}



