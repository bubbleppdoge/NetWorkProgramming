using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class LoginCtrl : MonoBehaviour {

	public InputField IDInput;          //id输入框
	public InputField pwInput;          //密码输入框
    public Text tipText;                //登录提示文本

	private bool canLogin = false;      //登录标志
    private string tipStr;              //获取正确登录提示

	void Awake()
    {
        Time.timeScale = 1f;

        Client.instance.onReceiveMsg += OnReceiveMsg;                   //加入委托，正确执行接收数据后的反馈

        tipStr = " ";
        tipText.text = " ";
        canLogin = false;
    }

	public void LoginBtn()                                              //登录按钮
	{
		UserData uData = new UserData (IDInput.text, pwInput.text);     //获取数据
		string msgToSend = JsonMapper.ToJson (uData);                   //转换为json字符串
        Client.instance.SetUserName(uData.id);                          //获取玩家名字
        Client.instance.SendMessage (msgToSend);                        //发送数据给服务端
	}

	void Update()
	{
        tipText.text = tipStr;                                          //更新提示文本

		if (canLogin) {                                                 //登录成功执行
			canLogin = false;
			SceneManager.LoadScene (1);
		}
	}

	public void ExitBtn()                                               //退出按钮
	{
		Application.Quit ();
	}

	void OnReceiveMsg(string jsonStr)                                   //委托函数
	{
		UserData uData = JsonUtility.FromJson<UserData>(jsonStr);       //获取登录信息
		string action = uData.action;                                   //获取行为
		if (action == "newLogin" || action == "canLogin") {             //根据行为执行登录操作
			canLogin = true;
            tipStr = "login succeed";
            Client.instance.onReceiveMsg -= OnReceiveMsg;               //成功登录后去除委托
		} else if (action == "cannotLogin")
            tipStr = "invalid password";
		else
            tipStr = "unkown error";
	}
}
