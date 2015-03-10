using UnityEngine;
using System.Collections;

public class Attack1 : AbilityBaseClass {

    float endTime2 = 0.65f;//技能1还剩多长时间时按X可以连接到技能2。
    public float attacked1Force = 50f;//角色攻击时，如果按下方向键所增加的力
    public float attacked1MaxMove = 4.5f,attacked4MaxMove=3f;
    //保存播放动画的名称
    public string attack1 = "attack2", attack2= "sweep",attack3= "attack1", attack4= "sweepBack"; 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))//如果按下X
        {
            if (actState.isRunIdle&&!IsName(attack1))//是站立或跑动，以及不是attack1
            {
                anim.SetTrigger(attack1);//播放attack1
                AttackedMaxSpeed(attacked1MaxMove);//限制攻击时的最大速度
            }
            else if (IsName(attack1) &&!IsName(attack2) &&GetAnimEndTime<0.8)//是攻击1，并且不是攻击2。动画距离结束时间少于0.65秒(按下X)
            {
                anim.SetTrigger(attack2);//播放attack2
            }
            else if(IsName(attack2) && !IsName(attack3) && GetAnimEndTime<0.8)
            {
                anim.SetTrigger(attack3);
            }
            else if(IsName(attack3)&&!IsName(attack4)&&GetAnimEndTime<0.8)
            {
                anim.SetTrigger(attack4);
            }
        }
    }
	void FixedUpdate()
    {
        if(IsName(attack2) &&GetAnimRate<0.5f)//玩家是否按下←或→。并且是在播放动画attack2
        {
            AddForce(attacked1Force);
            AttackedMaxSpeed(attacked1Force);
            AttackedMaxSpeed(attacked4MaxMove);//限制移动速度
        }
        else if(IsName(attack3)&& GetAnimRate < 0.5f) //GetAnimRate >0.375f&&GetAnimRate<0.75)//动画播放到百分之37到百分之75
        {
            AddForce(attacked1Force);
            AttackedMaxSpeed(attacked4MaxMove);//限制移动速度
        }
        else if(IsName(attack4)&&GetAnimRate<0.5f)
        {
            AddForce(attacked1Force);
            AttackedMaxSpeed(attacked4MaxMove);//限制移动速度
        }
    }
    void AddForce(float force)//给角色添加力
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (actState.rightArrow)
        {
            rigid.AddForce(new Vector2(force, 0));
        }
        else if (actState.leftArrow)
        {
            rigid.AddForce(new Vector2(-force, 0));
        }
    }
    void AttackedMaxSpeed(float maxSpeed)//限制攻击时的最大速度
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        Vector2 velocity = rigid.velocity;
        rigid.velocity = new Vector2(Mathf.Sign(velocity.x) * Mathf.Min(maxSpeed, Mathf.Abs(velocity.x)), velocity.y);//设置角色攻击时最大力不超过限定最大力
    }
}
