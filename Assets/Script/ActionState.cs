using UnityEngine;
using System.Collections;

public class ActionState : MonoBehaviour {//保存角色的动作状态
    private Animator anim;
    [HideInInspector]
    public bool rightArrow, leftArrow;//玩家是否按下左右键
    [HideInInspector]//隐藏在InInspector面板显示
    public bool rightSide = true;//控制角色的面相,.默认是右边
    [HideInInspector]
    public bool isRunIdle = false;//玩家是否处于跑动或空闲状态
    public Transform groundCheck;
    [HideInInspector]
    public bool isGround;//是否在地面···
    [HideInInspector]
    public float unmatchedTime = 0.0f;//无敌时间
    void Start () {
        anim = GetComponent<Animator>();
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
        isRunIdle = IsName("run") || IsName("idle");//玩家是否处于跑动或空闲状态
    }
    bool IsName(string name)//判断当前播放的是否某个动画名称
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    void FixedUpdate()
    {
        isGround = Physics2D.Linecast(groundCheck.position, transform.position, 1 << LayerMask.NameToLayer("ground"));
    }
}
