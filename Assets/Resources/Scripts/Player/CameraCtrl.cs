using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

	public Transform viewPoint;		//视点
	[Range(1,5)]
	public int moveSpeed = 5;		//视角移动速度
	private Vector3 offset;			//摄像机与玩家偏移量

	private GameCtrl gCtrl;			//游戏控制脚本

	void Start () {
		offset = viewPoint.position - transform.position;

		gCtrl = GameObject.Find ("GameCtrl").GetComponent<GameCtrl> ();
	}

	void Update () {
		if (gCtrl.gameOver || gCtrl.gamePause)										//游戏结束和游戏暂停脚本不可用
			return;
		transform.position = viewPoint.position - offset;							//保持摄像机与玩家距离

		float mouseX = Input.GetAxis("Mouse X") * moveSpeed ;						//鼠标滑动移动视角
		float mouseY = Input.GetAxis("Mouse Y") * moveSpeed ;

		transform.RotateAround(viewPoint.position, Vector3.up, mouseX);				//根据移动方向改变摄像机看向玩家的角度
		transform.RotateAround(viewPoint.position, viewPoint.right, -mouseY);
		offset = viewPoint.position - transform.position;							//重置偏移量，保证摄像机跟玩家距离始终保持一致
	}

	public void SetSpeed(int speed)													//设置速度，暂停页改变速度调用
	{
		moveSpeed = speed;
	}
}
