using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    // Use this for initialization
    public float maxSpeed=10;//玩家移动速度
    public float moveForce = 100;//移动时每次增加的力
    [HideInInspector]//属性不显示在Inspector。虽然不显示，但是仍然会取属性不显示在Inspector面板中的默认值···记得去掉查看/修改
    public Animator anim;
    [HideInInspector]
    public bool rightArrow, leftArrow;//玩家是否按下左右键
    [HideInInspector]
    public bool rightSide=true;//控制角色的面相,.默认是右边
    private Rigidbody2D rigid;//角色的刚体组件
    void Start () {
        anim = GetComponent<Animator>();
        rigid =GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        leftArrow = rightArrow = false;//默认是false··无视这点效率了
        if (Input.GetKey(KeyCode.RightArrow))//玩家是否按下→方向键
        {
            rightArrow = true;
            if(!rightSide) Flip();//调用反转函数
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) //玩家是否按下←方向键
        {
            leftArrow = true;
            if (rightSide) Flip();
        } 
        anim.SetBool("run", rightArrow || leftArrow);//按下左右任意一个都true
    }
    void FixedUpdate()
    {
        if(leftArrow)
        {
            rigid.AddForce(new Vector2(-moveForce,0));//给角色添加力
        }else if(rightArrow)
        {
            rigid.AddForce(new Vector2(moveForce, 0));
        }
    }
    void Flip()
    {
        Vector3 vt3 = transform.localScale;
        vt3.x *= -1;
        transform.localScale = vt3;//修改父物体x缩放为反方向
        rightSide = !rightSide;//设置角色面相相反
    }
}
