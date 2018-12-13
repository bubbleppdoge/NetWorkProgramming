using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class GameBtnCtrl : MonoBehaviour {


	private GameCtrl gCtrl;						//游戏控制脚本
	private CameraCtrl camCtrl;					//摄像机控制脚本

	public Text mSpeedText;						//暂停页面视角移动速度文本
	public Slider mSpeedSlider;					//速度滑动条

	void Start()
	{
		gCtrl = GetComponent<GameCtrl> ();
		camCtrl = Camera.main.GetComponent<CameraCtrl> ();

		mSpeedText.text = "Mouse Move Speed: " + camCtrl.moveSpeed.ToString ();
		mSpeedSlider.value = camCtrl.moveSpeed;
	}

	void Update()
	{
		mSpeedText.text = "Mouse Move Speed: " + mSpeedSlider.value.ToString();		//根据滑动条的控制实施更新文本
	}

	public void AgainBtn()															//重玩按钮
	{
		SceneManager.LoadScene(2);
	}

	public void MenuBtn()															//返回菜单按钮
	{
		SceneManager.LoadScene(1);
	}

	public void ContinueBtn(GameObject pausePage)									//继续按钮
	{
		Time.timeScale = 1;															//恢复游戏正常速度

		pausePage.SetActive (false);												//关闭暂停页
		gCtrl.gamePause = false;

		Cursor.lockState = CursorLockMode.Locked;									//隐藏指针
		Cursor.visible = false;

		camCtrl.SetSpeed ((int)mSpeedSlider.value);									//根据设置改变视角移动速度
	}
}
