using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

	public Transform viewPoint;		//视点

	private Animator anim;			//动画状态机
	private RayCastCtrl rCtrl;		//射线检测脚本
	private GameCtrl gCtrl;			//游戏控制脚本

	private Vector3 standInHalfSlop;//半坡上的位置

	public int hp = 100;			//玩家血量
	public float speed = 5f;		//玩家移动速度
	public float waitForAttack = 2f;//攻击间隔
	private float curWaitTime;		//间隔计时

	[HideInInspector]
	public bool attack;				//攻击标志
	[HideInInspector]
	public bool beAttacked;			//被攻击标志
	[HideInInspector]
	public bool die;				//死亡标志

	void Start()
	{
		curWaitTime = waitForAttack;	//第一次攻击不需间隔

		attack = false;
		beAttacked = false;
		die = false;

		anim = GetComponent<Animator> ();
		rCtrl = GetComponentInChildren<RayCastCtrl> ();
		gCtrl = GameObject.Find("GameCtrl").GetComponent<GameCtrl> ();
	}

	void Update()
    {
        if (gCtrl.gameOver && !die)
        {
            anim.SetTrigger("GameOver");
            return;
        }

        if (gCtrl.gameOver || gCtrl.gamePause)					//游戏结束和游戏暂停停止执行
			return;

        curWaitTime += Time.deltaTime;							//攻击计时增加

		float h = Input.GetAxisRaw ("Horizontal");				//移动方向
		float v = Input.GetAxisRaw ("Vertical");

		Attack ();												//攻击

		Move (h, v);											//移动

		if (beAttacked) {										//被攻击
			anim.SetTrigger ("BeAttacked");
			beAttacked = false;
		}

		if (hp <= 0 && !die) {									//死亡
			anim.SetTrigger ("Die");
			die = true;
		}
	}

	void Move(float h, float v)
	{
		h = attack ? 0 : h;																				//攻击过程无法移动
		v = attack ? 0 : v;

		anim.SetFloat ("speed", Mathf.Abs(v) + Mathf.Abs(h));											//根据速度控制移动动画

		if (h != 0 || v != 0 && rCtrl.IsClimbed) {														//记录玩家在半坡上的位置
			standInHalfSlop = transform.position;
		}
		if (rCtrl.IsClimbed && h == 0 && v == 0) {														//玩家不控制且站于半坡上时保持不动（受重力影响会往下滑动，因此固定位置）
			transform.position = standInHalfSlop;
		} 
		if (rCtrl.IsGrounded && !attack && (v != 0 || h != 0)) {										//站于地面并且不是攻击状态并且移动才能改变玩家位置
			transform.LookAt (viewPoint.forward * v + viewPoint.right * h + transform.position);		//玩家面向移动方向
			if (v != 0 && h != 0) {																		//斜向移动时，根据三角力的合成，大小应为原本sqrt(2)才能保持匀速
				v /= 1.414f;
				h /= 1.414f;
			}
			transform.Translate(Vector3.forward * Mathf.Abs(v) * speed * Time.deltaTime, Space.Self);	//移动
			transform.Translate(Vector3.forward * Mathf.Abs(h) * speed * Time.deltaTime, Space.Self);
		}
	}

	void Attack()
	{
		if (Input.GetMouseButton (0) && curWaitTime >= waitForAttack) {									//按下鼠标左键并且能够攻击才执行攻击
			anim.SetBool ("Attack", true);																//播放攻击动画
			curWaitTime = 0;																			//攻击计时归零
			attack = true;
		} else {
			AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);								//获取当前攻击播放动画
			if (info.IsName ("2HAttack") && info.normalizedTime >= 0.9f) {								//攻击动画播放完毕才更改攻击标志（这样才能正确结算）
				anim.SetBool ("Attack", false);
				attack = false;
			}
		}
	}

	public void TakeDamage(int damage)																	//受伤时减少血量
	{
		hp -= damage;
	}

	void OnTriggerEnter(Collider other)                                                                 //拾取方块时触发
	{
		if (other.tag == "Cube") {
            gCtrl.cubeNum += 1;
			gCtrl.score += 100;
			Destroy (other.gameObject);
		}
	}
}
