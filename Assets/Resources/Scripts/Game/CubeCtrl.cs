using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCtrl : MonoBehaviour {

	void Update () {
		transform.eulerAngles += new Vector3 (0, 45, 0) * Time.deltaTime;
	}
}
