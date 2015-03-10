using UnityEngine;
using System.Collections;

public class Attack1 : AbilityBaseClass {

    float endTime2 = 0.65f;//技能1还剩多长时间时按X可以连接到技能2。
    public float attacked1Force = 50f;//角色攻击时，如果按下方向键所增加的力
    public float attackedMaxForce = 4.5f;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))//如果按下X
        {
            if (actState.isRunIdle&&!IsName("attack1"))//是站立或跑动，以及不是attack1
            {
                anim.SetTrigger("attack1");//播放attack1
                Rigidbody2D rigid = GetComponent<Rigidbody2D>();
                Vector2 velocity = rigid.velocity;
                rigid.velocity=new Vector2(Mathf.Sign(velocity.x) * Mathf.Min(attackedMaxForce, Mathf.Abs(velocity.x)), velocity.y);//设置角色攻击时最大力不超过限定最大力
            }
            if (IsName("attack1") &&!IsName("attack2")&&GetAnimEndTime<0.65)//是攻击1，并且不是攻击2。动画距离结束时间少于0.65秒(按下X)
            {
                anim.SetTrigger("attack2");//播放attack2
            }
        }
    }
	void FixedUpdate()
    {
        if(IsName("attack2")&&GetAnimRate<0.5f)//玩家是否按下←或→。并且是在播放动画1
        {
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            if(actState.rightArrow)
            {
                rigid.AddForce(new Vector2(attacked1Force, 0));
            }else if(actState.leftArrow)
            {
                rigid.AddForce(new Vector2(-attacked1Force, 0));
            }
        }
    }
}
