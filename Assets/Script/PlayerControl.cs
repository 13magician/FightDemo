using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]//属性不显示在Inspector
    public Animator anim;
    [HideInInspector]
    public bool rightArrow, leftArrow;//玩家是否按下左右键
    [HideInInspector]
    public bool rightSide;//控制角色的面相
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        rightArrow = Input.GetKey(KeyCode.RightArrow);//玩家是否按下→方向键
            leftArrow = Input.GetKey(KeyCode.LeftArrow);//玩家是否按下←方向键
        anim.SetBool("run", rightArrow || leftArrow);//按下左右任意一个都true
        if(rightArrow && !rightSide)//如果按下→键以及面向不是右边
        {
            Flip();//调用反转函数
        }else if(leftArrow && rightSide) //如果按下←键以及面向是右边
        {
            Flip();
        }
    }
    void FixedUpdate()
    {

    }
    void Flip()
    {
        rightSide = !rightSide;//设置角色面相相反
        Vector3 vt3 = transform.localScale;
        vt3.x *= -1;
        transform.localScale = vt3;
        Debug.Log(transform.localScale);
    }
}
