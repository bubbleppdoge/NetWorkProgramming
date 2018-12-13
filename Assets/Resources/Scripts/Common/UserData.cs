using System;
using System.Collections;
using System.Collections.Generic;

public class UserData {         //玩家信息
	public string action;       //行为
	public string id;           //id
	public string password;     //密码

	public UserData(string _id, string _password)       //初始化
	{
		action = ACTIONTYPE.login.ToString ();
		id = _id;
		password = _password;
	}
}

[Serializable]
public class RListArray         //排行榜数据
{
    public List<RList> data;
}

[Serializable]
public class RList              //排行榜单项数据
{
	public string name;         //名字
	public int score;           //分数
    public string action;       //行为

    public RList(string _name, int _score)      //初始化
    {
        action = ACTIONTYPE.updateRankingList.ToString();
        name = _name;
        score = _score;
    }
}

public enum ACTIONTYPE          //行为表
{
	updateRankingList,
	setRankingList,
	login
}