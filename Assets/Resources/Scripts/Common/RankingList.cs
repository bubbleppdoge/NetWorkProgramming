using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using LitJson;
using System.Linq;

public class RankingList : MonoBehaviour
{
    public Text rankText;   							//名次文本
    public Text nameText;   							//姓名文本
    public Text scoreText;  							//分数文本

    private int NumOfRank = 10;							//排行榜显示数量
	private List<Data> rankList = new List<Data>();		//用来存储排行榜数据

    private string jStr;                                //接收json字符串
    private bool recv = false;                          //接收成功标志

    private RListArray rListData;                       //排行版数据

	void Awake()
    {
        Client.instance.onReceiveMsg += OnReceiveMsg;   //加入委托
	}

    void Start()
    {
        rankText.text = "Rank";
        nameText.text = "Name";
        scoreText.text = "Score";
        rListData = new RListArray();

        string action = "{\"action\":\"" + ACTIONTYPE.setRankingList.ToString() + "\"}";    //脚本执行时发送数据给服务端更新排行榜
        Client.instance.SendMessage(action);
    }

    void Update()
    {
        if (recv)       //接收到数据
        {
            recv = false;
            rListData = JsonUtility.FromJson<RListArray>(jStr); //转换为排行榜数据
            ShowRankingList();                                  //显示排行榜
        }
    }

	void OnReceiveMsg(string jsonStr)                           //委托事件
	{
        jStr = jsonStr;                                         //获得字符串
		Client.instance.onReceiveMsg -= OnReceiveMsg;           //撤销事件
        recv = true;
	}

    public void ShowRankingList()                               //显示数据
	{
		rankText.text = "Rank";
		nameText.text = "Name";
		scoreText.text = "Score";
		if (rListData.data.Count != 0) {
			int i = 1;
            foreach (RList item in rListData.data) {
                rankText.text += "\n" + i.ToString();
                nameText.text += "\n" + item.name;
                scoreText.text += "\n" + item.score;
                i++;
			}
		}
		else
		{
			rankText.text += "\nnull";
			nameText.text += "\nnull";
			scoreText.text += "\nnull";
		}
    }

}