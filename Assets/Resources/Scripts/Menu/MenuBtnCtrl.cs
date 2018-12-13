using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBtnCtrl : MonoBehaviour {

	public GameObject cImg;					//开始结束按钮旁边的指标贴图
    public Text hellowText;                 //菜单界面名字显示

	void Start () 							//初始化
	{
		Time.timeScale = 1f;
		cImg.SetActive (false);
        hellowText.text = "Hello! " + Client.instance.userName;
	}

	public void StartBtn()					//开始按钮
	{
		SceneManager.LoadScene (2);
	}

	public void ExitBtn()					//结束按钮
	{
        SceneManager.LoadScene(0);
    }

	public void OnPointEnter(Text self)		//只有鼠标指针放在按钮上才显示变化
	{
		self.color = Color.red;
		self.fontSize = 35;
		cImg.SetActive (true);
		cImg.gameObject.transform.position = self.transform.position - Vector3.right * 60;
	}

	public void OnPointExit(Text self)		//鼠标离开按钮时
	{
		self.color = Color.white;
		self.fontSize = 27;
		cImg.SetActive (false);
	}

	public void OnPointDown(Text self)		//鼠标点击时
	{
		self.color = Color.gray;
		self.fontSize = 27;
	}
}
