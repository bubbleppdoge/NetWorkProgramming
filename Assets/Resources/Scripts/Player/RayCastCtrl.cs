using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastCtrl : MonoBehaviour {

	private RaycastHit hit;				//向下射线检测地面

	public CapsuleCollider pCollider;	//玩家碰撞盒

	public float rayLength;				//向下射线长度
	public float addRayLength;			//四周射线增加长度（四周射线长度默认为玩家碰撞盒半径）
	public int rayNum;					//四周射线数量

	private bool isGround;				//地面站立标志
	private bool canClimb;				//爬坡标志

	void Update () {
		if (Physics.Raycast (transform.position, Vector3.down, out hit, rayLength)) {		//射线检测到地面改变标志
			isGround = true;
			if (hit.distance > rayLength / 2 && hit.distance < rayLength) {					//位于坡上，射线比站立地面较长
				RaycastHit sHit;															//四周射线检测身旁是否为坡
				for (int i = 0; i < rayNum; i++) {											//射线环绕角色一周
					float x = Vector3.Magnitude (transform.forward) * Mathf.Cos (Mathf.Deg2Rad * 360 / rayNum * i);
					float z = Vector3.Magnitude (transform.forward) * Mathf.Sin (Mathf.Deg2Rad * 360 / rayNum * i);
					if (Physics.Raycast (transform.position, (new Vector3 (x, 0, z)).normalized, out sHit, pCollider.radius + addRayLength)) {
						canClimb = true;													//射线检测成功即为坡上
						break;
					}
				}
			} else
				canClimb = false;
		} else
			isGround = false;
	}

	public bool IsGrounded						//站立地面标志获取
	{
		get{ return isGround;}
	}

	public bool IsClimbed						//位于坡上标志获取
	{
		get{ return canClimb;}
	}

	void OnDrawGizmos()							//在scene界面显示射线
	{
		Gizmos.color = new Color (1, 0, 0);
		Gizmos.DrawRay (transform.position, Vector3.down * rayLength);

		for (int i = 0; i < rayNum; i++) {
			float x = Vector3.Magnitude (transform.forward) * Mathf.Cos (Mathf.Deg2Rad * 360 / rayNum * i);
			float z = Vector3.Magnitude (transform.forward) * Mathf.Sin (Mathf.Deg2Rad * 360 / rayNum * i);
			Gizmos.DrawLine (transform.position, transform.position + (new Vector3 (x, 0, z)).normalized * (pCollider.radius + addRayLength));
		}

	}


}
