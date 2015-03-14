using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class Assault : AbilityBaseClass {

    private string abilityName="Assault";//一定要设置类技能名称
    public string animName1 = "assault";
    public float assaultForceX = 30f, assaultForceY = 10f;
    public float assaultSpeed = 0.15f, assaultMaxSpeed=15f;
    public float upTime = 0.35f;//坐标上升时间率
    public float assaultEntTime = 0.66f;//冲刺结束时间率
    public float beatBackX = 1000f, beatBackY = 300f;
    public List<Monster> monsterList = new List<Monster>();//用来存储这次被击中的怪物
    // Use this for initialization
    protected override void AbiStart()
    {
       
    }
    protected override void TriggerAbility(Transform hit)//继承基类的技能触发接口
    {
        Monster monster = hit.GetComponent<Monster>();
        if(monster!=null && !monsterList.Contains(monster))//如果怪物类脚本不为空（他是一个怪物）。以及他没被击中
        {
            monsterList.Add(monster);//添加到被击中列表
            monster.WasAttacked(0.9f);//播放被攻击动画0.9秒
            monster.currentHP -= 5f;//减少血量
            GameObject effect = Instantiate(player.effect, hit.position, Quaternion.identity) as GameObject;//克隆一个特效，旋转对齐于世界或父类
            GameObject effect2 = Instantiate(player.effect, hit.position, Quaternion.identity) as GameObject;//克隆一个特效，旋转对齐于世界或父类
            effect.GetComponent<Effect>().bindEffect(hit.transform, "light",1f);// 设置这个特效的绑定对象。并且让这个特效播放光动画
            effect2.GetComponent<Effect>().bindEffect(hit.transform,  "blood",1f);
            CheckEffectSide(hit,effect);//根据玩家和怪物的位置重设特效的side面相
            CheckEffectSide(hit,effect2);
            if(transform.position.x>hit.position.x)
            {
                hit.GetComponent<Rigidbody2D>().AddForce(new Vector2( -beatBackX, beatBackY));
            }
            else hit.GetComponent<Rigidbody2D>().AddForce(new Vector2(beatBackX, beatBackY));

            //iTween.MoveTo(hit.gameObject, hit.position + new Vector3(5, 3, 0), 1);//用ITWEEN移动。被移出墙外
            //else iTween.MoveTo(hit.gameObject, hit.position + new Vector3(-5, 3, 0), 1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && IsName("run"))
        {
            StartCoroutine(SetTrigger(animName1));//设置真，0.25秒后变成假
        }
    }
    void FixedUpdate()
    {
        if(IsName(animName1))//如果处于释放技能动画中
        {
            //AddForceX(assaultForceX);
            // GetComponent<Rigidbody2D>().velocity = new Vector2(assaultSpeed, 1f);
            //AddForceY(assaultForceY);
            if (GetAnimRate < assaultEntTime)//如果冲刺时间还没结束
            {
                if (actState.rightSide) iTween.MoveUpdate(gameObject, transform.position + new Vector3(assaultSpeed, 0, 0), 0f);//如果是面向右边就正数
                else iTween.MoveUpdate(gameObject, transform.position + new Vector3(-assaultSpeed, 0, 0), 0f) ;//否则就是负数
            }
            if (GetAnimRate >assaultEntTime) //大于冲刺时间（惯性力）
            {
                if (actState.rightSide) iTween.MoveUpdate(gameObject, transform.position + new Vector3(assaultSpeed/2, 0, 0), 0f);//如果是面向右边就正数
                else iTween.MoveUpdate(gameObject, transform.position + new Vector3(-assaultSpeed/2, 0, 0), 0f);//否则就是负数
                //  iTween.MoveUpdate(gameObject, transform.position + new Vector3(0, 0.1f, 0), 0f);//往上移动
            }
        }
        else if(monsterList.Count>0)//如果播放完动画，并且怪物列表里有怪物
        {
            monsterList.Clear();//清除
        }
    }
    public override string AbilityName
    {
        get
        {
            return abilityName;   
        }

        set
        {
            abilityName = value;
        }
    }
}
