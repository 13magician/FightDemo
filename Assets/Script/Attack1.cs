using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Attack1 : AbilityBaseClass {

    float endTime2 = 0.65f;//技能1还剩多长时间时按X可以连接到技能2。
    public float attacked1Force = 50f;//角色攻击时，如果按下方向键所增加的力
    public float attacked1MaxMove = 4.5f,attacked4MaxMove=2f;
    string abilityName = "Attack1";//这是类的名字，一定要定义技能的名字
    public AudioClip attackAudio1, attackAudio2, attackAudio3, attackAudio4;//4个攻击音效
    public override string AbilityName { get { return abilityName; } set { abilityName = value; } }//名字的属性··蛋疼。已经放在基类。是抽象。要重写
    //delegate void TriggerAbility();//定义一个委托··放到技能基类。好像不需要这个··

   // Dictionary<string, TriggerAbility> triggerAbility = new Dictionary<string, TriggerAbility>();//定义一个mapping···放到基类
    //保存播放动画的名称
    public string attack1 = "attack2", attack2= "sweep",attack3= "attack1", attack4= "sweepBack";
    public float attack1Posture = 0.4f, attack2Posture = 0.7f, attack3Posture = 0.7f;//攻击1后摇
    public float keyDuration;//按键持续时间
    protected override void  AbiStart()//重写基类的AbiStrat函数···是否要考虑换下名字，比如Init···
    {
        attackAudio1 = Resources.Load("attack1-4/attack1") as AudioClip;//读取本地资源文件
        attackAudio2 = Resources.Load("attack1-4/attack2") as AudioClip;
        attackAudio3 = Resources.Load("attack1-4/attack3") as AudioClip;
        attackAudio4 = Resources.Load("attack1-4/attack4") as AudioClip;
    }
    protected override void TriggerAbility(Transform hit)//技能碰撞的接口
    {
        if (hit.GetComponent<Monster>() != null)//如果有怪物类脚本
        {
            Rigidbody2D rigid = hit.GetComponent<Rigidbody2D>();
            GameObject effect = Instantiate(player.effect, hit.position,Quaternion.identity) as GameObject;//克隆一个特效，旋转对齐于世界或父类
            GameObject effect2 = Instantiate(player.effect, hit.position, Quaternion.identity) as GameObject;//克隆一个特效，旋转对齐于世界或父类
            effect.GetComponent<Effect>().bindEffect(hit.transform, "light",1f);// = hit.transform;//设置这个特效的绑定对象。被触发的单位
            effect2.GetComponent<Effect>().bindEffect(hit.transform, "blood",1f);// = hit.transform;//设置这个特效的绑定对象。被触发的单位
            if (IsName(attack4)) hit.GetComponent<Monster>().currentHP -= 4;//如果是重击动画就减4
            else hit.GetComponent<Monster>().currentHP -= 2;
            hit.GetComponent<Monster>().WasAttacked(0.65f, transform);//调用怪物类的被攻击接口。被攻击动画持续0.65秒
            CheckEffectSide(hit,effect);//检测特效的左右缩放
            CheckEffectSide(hit, effect2);//检测特效的左右缩放
            rigid.velocity = new Vector2(0, rigid.velocity.y);
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
        keyDuration -= Time.deltaTime;//按键时间减少
        if (player.playState.isGround)//判断玩家是否在地面
        {
            if (Input.GetKeyDown(KeyCode.X))//如果按下X
            {
                if (!IsName(attack4)) //如果不是处于最后一击动画时，不然很容易又播放1
                keyDuration = 0.5f;//按键持续时间=0.5
            }
            if (actState.isRunIdle && !IsName(attack1) && keyDuration > 0f)//是站立或跑动，以及不是attack1
            {
                anim.Play(attack1);//直接播放攻击1动画
                AudioSource.PlayClipAtPoint(attackAudio1, transform.position);//播放攻击音效
                keyDuration = 0f;
                AttackedMaxSpeed(attacked1MaxMove);//限制攻击时的最大速度
                player.playState.unmatchedTime += 0.2f;//添加0.5秒的无敌
                                                   //  StartCoroutine(SetTriggetFlase(attack1));//0.25秒后设置触发为假。不加StartCoroutine也不报错···
            }
            else if (IsName(attack1) && !IsName(attack2) && keyDuration > 0f)//是攻击1，并且不是攻击2。按键持续时间大于0
            {
                if (GetAnimRate > attack1Posture)//处于attack1后摇时，就播放attack2
                {
                    AudioSource.PlayClipAtPoint(attackAudio2, transform.position);
                    anim.Play(attack2);
                    keyDuration = 0f;
                }
            }
            else if (IsName(attack2) && !IsName(attack3) && keyDuration > 0f)
            {
                if (GetAnimRate > attack2Posture)//attack1Posture大于后摇
                {
                    anim.Play(attack3);
                    AudioSource.PlayClipAtPoint(attackAudio3, transform.position);
                    keyDuration = 0f;
                }
            }
            else if (IsName(attack3) && !IsName(attack4) && keyDuration > 0f)
            {
                if (GetAnimRate > attack3Posture)//attack1Posture大于后摇
                {
                    anim.Play(attack4);
                    keyDuration = 0f;
                    AudioSource.PlayClipAtPoint(attackAudio4, transform.position);
                }
            }
        }
    }

    //void DurationAnimTrue(string _animName,float durationTime= maxInterval)//让某个动画持续播放多长时间
    //{
    //    if(durationPlyAnim!=_animName)
    //    {
    //        if(durationPlyAnim!="")
    //        anim.SetBool(durationPlyAnim, false);//如果新持续的动画不等于之前的那个。就设成假
    //    }
  
 
    //    if(_animName != "")
    //    {
    //        durationPlyAnim = _animName;
    //        durationPlyTime = durationTime;
    //        anim.SetBool(durationPlyAnim, true);
    //    }
    //}
    void FixedUpdate()
    {
        if (IsName(attack1) && GetAnimRate < 0.35f)
        {
            AddKeyForceX(attacked1Force);
            AttackedMaxSpeed(attacked1Force);
            AttackedMaxSpeed(attacked4MaxMove);
        }
            if (IsName(attack2) && GetAnimRate < 0.5f)//玩家是否按下←或→。并且是在播放动画attack2
        {
            AddKeyForceX(attacked1Force);
            AttackedMaxSpeed(attacked1Force);
            AttackedMaxSpeed(attacked4MaxMove);//限制移动速度
        }
        else if (IsName(attack3) && GetAnimRate < 0.5f) //GetAnimRate >0.375f&&GetAnimRate<0.75)//动画播放到百分之37到百分之75
        {
            AddKeyForceX(attacked1Force);
            AttackedMaxSpeed(attacked4MaxMove);//限制移动速度

        }
        if (IsName(attack4) && GetAnimRate < 0.5f)
        {
            AddKeyForceX(attacked1Force);
            AttackedMaxSpeed(attacked4MaxMove);//限制移动速度
        }
        //if (durationPlyTime >= 0.0f)//这里是让某个动画持续播放多少秒
        //{
        //    if(durationPlyAnim!="")
        //    anim.SetBool(durationPlyAnim, true);
        //    durationPlyTime -= Time.fixedDeltaTime;
        //}
        //else if (durationPlyAnim != "")//如果动画名不为空
        //{
        //    anim.SetBool(durationPlyAnim, false);//设置这个动画为假
        //    durationPlyAnim = "";
        //    durationPlyTime = 0.0f;
        //}
    }

    void AttackedMaxSpeed(float maxSpeed)//限制攻击时的最大速度--技能基类
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        Vector2 velocity = rigid.velocity;
        rigid.velocity = new Vector2(Mathf.Sign(velocity.x) * Mathf.Min(maxSpeed, Mathf.Abs(velocity.x)), velocity.y);//设置角色攻击时最大力不超过限定最大力
    }
}
