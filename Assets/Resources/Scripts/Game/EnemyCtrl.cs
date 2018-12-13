using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour {

	public Transform player;			//玩家Transform
	public GameObject bloodPre;			//鲜血特效预制体
	private NavMeshAgent agent;			//AI代理
	private Animator anim;				//zombie动画
	private PlayerCtrl pCtrl;			//玩家控制脚本
	private GameCtrl gCtrl;				//游戏控制脚本

	public float backDis = 5f;			//后退距离  PS：AI被攻击后，后退超过这个距离，就会停止（并非必要，但是控制准确，可通过改变后退时间控制距离）
	public float backTime = 2f;			//后退的总时间（后退2s，会是很长的一段距离）
	private float curBackTime;			//当前后退时间

	public int damage = 10;				//攻击伤害
	public float waitForAttack = 1f;	//攻击间隔
	private float curWaitTime;			//间隔计时

	public float timeToChangeSpeed = 5f;//速度改变间隔
	public float onceSpeedValue = 0.3f;	//一次速度改变的大小

	private bool beAttacked;            //被攻击状态

    public int findRadius = 5;          //警戒范围，玩家到了该范围会发现玩家

    public List<Vector3> patrolPoints;  //巡逻点（基于自身坐标的偏移值）
    private List<Vector3> realPos;      //真实巡逻点（偏移值 + 自身初始坐标）
    private int pIndex = 0;             //当前走向的巡逻点
    private Vector3 initPos;            //记录游戏运行初始位置

    private bool findTarget = false;    //发现玩家标志
	public int moveSpeed = 5;           //发现玩家后移动速度
    public int patrolSpeed = 2;         //巡逻速度

	void Start()
	{
		curWaitTime = 0;
		curBackTime = 0;
		beAttacked = false;
        GetComponent<AudioSource>().Stop();
        agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		pCtrl = player.gameObject.GetComponent<PlayerCtrl> ();
		gCtrl = GameObject.Find ("GameCtrl").GetComponent<GameCtrl> ();

		findTarget = false;
		initPos = transform.position;
		realPos = new List<Vector3> ();
		foreach (Vector3 pPoint in patrolPoints) {
			realPos.Add (pPoint + transform.position);
		}
		agent.speed = patrolSpeed;
		agent.stoppingDistance = 0f;
	}

	void Update () {
		if (gCtrl.gameOver)     //游戏结束后停止执行
        {
            anim.SetTrigger("GameOver");
            return;
        }
		
		Patrol();

		if(!beAttacked && findTarget)
			agent.SetDestination (player.position);												//攻击目标（玩家）
		
		Attack ();																				//攻击

		BeAttacked ();																			//被攻击
	}

	void Attack()
	{
		if (Vector3.Distance (transform.position, player.position) > agent.stoppingDistance) {	//小于攻击距离则走向目标
			if (!beAttacked)
				anim.SetFloat ("Speed", agent.speed);											//播放行走动画
			curWaitTime = waitForAttack / 2;													//在走向目标过程中，攻击cd保持一半，加快接近目标后第一次攻击速度
		} else if (curWaitTime >= waitForAttack) {
			curWaitTime = 0;																	//攻击后重置攻击间隔								
			anim.SetTrigger ("Attack");															//播放攻击动画
			pCtrl.TakeDamage(damage);															//玩家受伤
			pCtrl.beAttacked = true;
		} else {
			transform.LookAt (new Vector3(player.position.x, transform.position.y, player.position.z));		//攻击时面向目标
			curWaitTime += Time.deltaTime;														//攻击间隔计时
			anim.SetFloat ("Speed", 0);															//播放站立动画
		}
	}

	void BeAttacked()
	{
		anim.SetBool ("BeAttacked", beAttacked);
		if (beAttacked) {
			curBackTime += Time.deltaTime;														//后退时间计时
			transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward, curBackTime /backTime);		//后退
			if (Vector3.Distance (transform.position, player.position) >= backDis - 0.02f) {	//后退到指定距离后停止
				beAttacked = false;
				gCtrl.score += 10;																//被攻击后玩家分数+1
				curBackTime = 0;																//重置后退计时
			}
		}
	}

	void Patrol()
	{
		if (Vector3.Distance (transform.position, player.position) <= findRadius) {
			findTarget = true;
			agent.speed = moveSpeed;
			agent.stoppingDistance = 3;
		} else {
			Vector3 curPoint = realPos [pIndex];
			agent.SetDestination (curPoint);
			if (Vector3.Distance (transform.position, curPoint) <= 3f)
				pIndex = (pIndex + 1) % realPos.Count;
		}
	}

	void OnTriggerEnter(Collider other)															//玩家武器上有触发器，玩家攻击碰撞后即受到攻击
	{
		if (other.tag == "Weapon" && pCtrl.attack) {											//只有玩家攻击时碰触才有效
			GameObject blood = Instantiate (bloodPre, transform.position + Vector3.up, Quaternion.identity);		//生成被攻击特效
            GetComponent<AudioSource>().Play();
			Destroy (blood, 1.2f);																//1.2s后销毁特效
			beAttacked = true;
		}
	}

	void OnDrawGizmos()							                                                //在scene界面显示巡逻坐标和警戒范围
	{
		Gizmos.color = new Color (1, 0, 0);
		float lineLength = 0.5f;

		foreach (Vector3 pPoint in patrolPoints) {
			Vector3 item = Application.isPlaying ? initPos + pPoint : transform.position + pPoint;
			Gizmos.DrawLine (new Vector3 (item.x + lineLength, item.y, item.z), new Vector3 (item.x - lineLength, item.y, item.z));
			Gizmos.DrawLine (new Vector3 (item.x, item.y, item.z + lineLength), new Vector3 (item.x, item.y, item.z - lineLength));
		}
		Gizmos.DrawWireSphere (transform.position, findRadius);
	}
}
