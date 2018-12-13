using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class GameCtrl : MonoBehaviour {

	[HideInInspector]
	public int score = 0;				//分数（击中敌人+1）
	[HideInInspector]
	public bool gameOver;				//游戏结束标志
	[HideInInspector]
	public bool gamePause;				//游戏暂停标志

	public float gameTime = 120f;		//游戏时间

	public Text timeText;				//游戏界面时间显示
	public Text hpNumText;				//游戏界面血量显示
	public Slider hpSlider;				//游戏界面血条显示
	public Image fillImage;				//游戏界面血条颜色控制

	public Text finScoreText;			//游戏结束页分数显示
    public Text idText;                 //游戏结束页名字显示

    public GameObject gameOverPage;		//游戏结束页
	public GameObject pausePage;		//游戏暂停页

	public PlayerCtrl pCtrl;            //玩家控制脚本	
    public RankingList rList;			//排行榜脚本

    [HideInInspector]
	public int cubeNum = 0;
	public Text cubeText;

	void Start () {                                                 //初始化数据
		Time.timeScale = 1;
        rList.gameObject.SetActive(false);
        timeText.text = "Time: " + gameTime.ToString();
		hpNumText.text = "100";
		hpSlider.value = 100;
		finScoreText.text = "0";
		fillImage.color = Color.green;
        idText.text = "Name";
        cubeText.text = "Cube: " + cubeNum.ToString() + " / 4";

        gameOverPage.SetActive (false);
		pausePage.SetActive (false);
		gameOver = false;
		gamePause = false;

		Cursor.visible = false;                                     //隐藏并锁定鼠标指针
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {
		if(!gameOver) gameTime -= Time.deltaTime;					//游戏时间减少
		if (gameTime <= 0) gameTime = 0;

		timeText.text = "Time: " + ((int)gameTime).ToString ();		//游戏界面UI显示控制
		hpNumText.text = pCtrl.hp.ToString();
		hpSlider.value = pCtrl.hp;
		cubeText.text = "Cube: " + cubeNum.ToString() + " / 4";
		fillImage.color = pCtrl.hp > 50 ? Color.green : pCtrl.hp > 20 ? Color.yellow : Color.red;	//颜色由血量改变而相应更改（绿黄红）

		if (Input.GetKeyDown (KeyCode.Escape)) {					//按下esc跳出暂停页
			GamePause ();
		}

		if (!gameOver && (gameTime <= 0 || pCtrl.die || cubeNum == 4)) {			//玩家血量为0或者游戏时间到或者收集指定方块完成则游戏结束
			GameOver ();
		}
	}

	void GamePause()
	{
		Time.timeScale = 0;

		Cursor.lockState = CursorLockMode.None;					//解除鼠标隐藏
		Cursor.visible = true;

		gamePause = true;										//显示暂停页
		pausePage.SetActive (true);
	}

	void GameOver()
	{
		gameOver = true;
		gameOverPage.SetActive (true);
        if (pCtrl.die)
            finScoreText.text = score.ToString();
        else
		    finScoreText.text = ((int)gameTime + score).ToString();                                 //更改最终分数文本
        idText.text = Client.instance.userName;                                                     //游戏结束页名字

        Cursor.lockState = CursorLockMode.None;                                                     //解除指针隐藏
		Cursor.visible = true;

        RList cData = new RList(Client.instance.userName, int.Parse(finScoreText.text));            //发送数据给服务端
        string msgToSend = JsonMapper.ToJson(cData);
        Client.instance.SendMessage(msgToSend);
    }
}
