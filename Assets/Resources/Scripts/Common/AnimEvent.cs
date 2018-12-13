using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour {

    public GameObject rListPage;        //排行榜

    public void showRListPage()         //动画结束事件：显示排行榜
    {
        rListPage.SetActive(true);
    }


                
}
