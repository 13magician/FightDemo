using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Attack1 : AbilityBaseClass {

    float endTime2 = 0.65f;//技能1还剩多长时间时按X可以连接到技能2。
    public float attacked1Force = 50f;//角色攻击时，如果按下方向键所增加的力
    public float attacked1MaxMove = 4.5f,attacked4MaxMove=2f;
    string abilityName = "Attack1";//一定要定义技能的名字
    public override string AbilityName { get { return abilityName; } set { abilityName = value; } }//名字的属性··蛋疼。已经放在基类。是抽象。要重写
    //delegate void TriggerAbility();//定义一个委托··放到技能基类。好像不需要这个··
    PlayerControl player;//玩家控制的角色··放到技能基类
   // Dictionary<string, TriggerAbility> triggerAbility = new Dictionary<string, TriggerAbility>();//定义一个mapping···放到基类
    //保存播放动画的名称
    public string attack1 = "attack2", attack2= "sweep",attack3= "attack1", attack4= "sweepBack";
    protected override void  AbiStart()//重写基类的AbiStrat函数···是否要考虑换下名字，比如Init···
    {
        player = GetComponent<PlayerControl>();
        if(!player.triggerAbility.ContainsKey(AbilityName))//如果玩家类的触发技能里没有我的技能
        {
            player.triggerAbility.Add(AbilityName, triggerAbility);//给玩家添加技能接口
        }
    }
    void triggerAbility(Transform hit)//技能碰撞的接口
    {
        if (hit.GetComponent<Monster>() != null)//如果有怪物类脚本
        {
            hit.GetComponent<Monster>().wasAttacked(0.65f);//调用怪物类的被攻击接口。被攻击动画持续0.65秒
            Rigidbody2D rigid = hit.GetComponent<Rigidbody2D>();
            GameObject effect = Instantiate(player.effect, hit.position,Quaternion.identity) as GameObject;//克隆一个特效，旋转对齐于世界或父类
            GameObject effect2 = Instantiate(player.effect, hit.position, Quaternion.identity) as GameObject;//克隆一个特效，旋转对齐于世界或父类
            effect.GetComponent<Effect>().bindEffect(hit.transform, 0.0f, "light");// = hit.transform;//设置这个特效的绑定对象。被触发的单位
            effect2.GetComponent<Effect>().bindEffect(hit.transform, 1.0f, "blood");// = hit.transform;//设置这个特效的绑定对象。被触发的单位
            if (IsName(attack4)) hit.GetComponent<Monster>().HP -= 4;//如果是重击动画就减4
            else hit.GetComponent<Monster>().HP -= 2;
            if (transform.position.x>hit.position.x)//如果玩家在怪物右边。就变换特效的缩放
            {
                effect.transform.localScale = new Vector2(-1 * effect.transform.localScale.x, effect.transform.localScale.y);//变换特效的缩放···名字有点长
                effect2.transform.localScale = new Vector2(-1 * effect2.transform.localScale.x, effect2.transform.localScale.y);//变换特效的缩放···名字有点长
            }
            if (Mathf.Abs( rigid.velocity.x) < 0.5f)//如果横轴速率小于1.就给他添加力
            {
                if (transform.position.x < hit.position.x)//如果玩家在怪物的左边，就添加正数的力
                {
                
                     rigid.AddForce(new Vector2(150f, 0f));
                    if (IsName(attack4))//如果是最后一击
                    {
                        rigid.AddForce(new Vector2(150f, 150f));
                    }
                }
                else
                {
                    rigid.AddForce(new Vector2(-150f, 0f));
                    if (IsName(attack4))//如果是最后一击
                    {
                        rigid.AddForce(new Vector2(-150f, 150f));
                    }
                }
            }
        }
        
    }
   
    void Update()
    {
        if (player.playState.isGround)//判断玩家是否在地面
        {
            if (Input.GetKeyDown(KeyCode.X))//如果按下X
            {
                if (actState.isRunIdle && !IsName(attack1))//是站立或跑动，以及不是attack1
                {
                    anim.SetTrigger(attack1);//播放attack1
                    AttackedMaxSpeed(attacked1MaxMove);//限制攻击时的最大速度
                }
                else if (IsName(attack1) && !IsName(attack2) && GetAnimEndTime < 0.8)//是攻击1，并且不是攻击2。动画距离结束时间少于0.65秒(按下X)
                {
                    anim.SetTrigger(attack2);//播放attack2
                }
                else if (IsName(attack2) && !IsName(attack3) && GetAnimEndTime < 0.8)
                {
                    anim.SetTrigger(attack3);
                }
                else if (IsName(attack3) && !IsName(attack4) && GetAnimEndTime < 0.8)
                {
                    anim.SetTrigger(attack4);
                }
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
         if(IsName(attack4) && GetAnimRate<0.5f)
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
    void AttackedMaxSpeed(float maxSpeed)//限制攻击时的最大速度--技能基类
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        Vector2 velocity = rigid.velocity;
        rigid.velocity = new Vector2(Mathf.Sign(velocity.x) * Mathf.Min(maxSpeed, Mathf.Abs(velocity.x)), velocity.y);//设置角色攻击时最大力不超过限定最大力
    }
}
