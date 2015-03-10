using UnityEngine;
using System.Collections;

public class ActionState : MonoBehaviour {//保存角色的动作状态
    [HideInInspector]
    public bool rightArrow, leftArrow;//玩家是否按下左右键
    [HideInInspector]//隐藏在InInspector面板显示
    public bool rightSide = true;//控制角色的面相,.默认是右边
    [HideInInspector]
    public bool isMove;//控制玩家是否可以移动或跳跃
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        leftArrow = rightArrow = false;//默认是false··无视这点效率了
        if (Input.GetKey(KeyCode.RightArrow))//玩家是否按下→方向键
        {
            rightArrow = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) //玩家是否按下←方向键
        {
            leftArrow = true;
        }
    }
}
