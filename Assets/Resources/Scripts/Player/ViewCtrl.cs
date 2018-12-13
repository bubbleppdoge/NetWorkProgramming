using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCtrl : MonoBehaviour {

	public Transform player;	//玩家Transform

	private GameCtrl gCtrl;		//游戏控制脚本

	void Start()
	{
		gCtrl = GameObject.Find ("GameCtrl").GetComponent<GameCtrl> ();
	}

	void Update () {
		if (gCtrl.gameOver || gCtrl.gamePause)		//在游戏结束和游戏暂停不执行
			return;
		transform.position = player.position;		//位置和旋转始终保持与玩家一致
		float mouseX = Input.GetAxis("Mouse X") * Camera.main.GetComponent<CameraCtrl>().moveSpeed ;
		transform.Rotate(Vector3.up, mouseX);
	}
}
