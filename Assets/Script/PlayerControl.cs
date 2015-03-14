using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PlayerControl : MonoBehaviour {//主要玩家控制角色
    //以后什么东西都新建一个父物体，trigger不精确。以后写个协同。0.1秒False
    // Use this for initialization
    public float moveMaxForce= 30f;//玩家移动速度
    public float moveForce = 4.5f;//移动时每次增加的力
    
    [HideInInspector]//属性不显示在Inspector。虽然不显示，但是仍然会取属性不显示在Inspector面板中的默认值···记得去掉查看/修改
    public Animator anim;
    private Rigidbody2D rigid;//角色的刚体组件
    [HideInInspector]
    public ActionState playState;//保存玩家的状态
    private Transform colliderAssist;//攻击辅助碰撞脚本对象
    public GameObject effect;//一个特效对象预设
    public Image HPBar;//血条
    public float countHP = 50f, currentHP = 50f;//主角的血量
    private string deathAnimName = "death";//死亡的动画名称
    [HideInInspector]
    public int killGreenWaterNum = 0;
    //public Transform mainCastAssist;//辅助投射的父物体
    //public  Transform[] castAssist;//4个辅助投射
    //public Transform mainColliderAssist;//攻击辅助碰撞
    public delegate void TriggerAbility(Transform hit);//定义一个委托，用于触发技能。主要给下面的Dictionary用
    public Dictionary<string, TriggerAbility> triggerAbility = new Dictionary<string, TriggerAbility>();//定义一个Dictionary(Mapping)。映射相应的技能
   
    void Start () {
        anim = GetComponent<Animator>();
        rigid =GetComponent<Rigidbody2D>();
        playState = GetComponent<ActionState>();//角色的行动状态
        colliderAssist = transform.Find("colliderAssist");//获取子物体碰撞辅助
        //添加技能组件
        //gameObject.AddComponent<Attack2>();//添加组件脚本，attack2。实际上控制技能2
        gameObject.AddComponent<Attack1>();
        gameObject.AddComponent<Assault>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playState.rightArrow && !playState.rightSide) Flip();//如果玩家按下右方向，并且面相不是右边。调用反转函数
        else if (playState.leftArrow && playState.rightSide) Flip();//否则判断如果玩家按下←方向，并且面相是右边。调用反转函数
       if(playState.isGround) anim.SetBool("run", playState.rightArrow || playState.leftArrow);//判断玩家是否在地面。按下左右任意一个都true
        HPBar.fillAmount = Mathf.Lerp(HPBar.fillAmount,currentHP / countHP,Time.deltaTime*countHP*0.05f+0.05f);//设置UI的血量显示
    }
    void FixedUpdate()
    {
        CheckUnmatched();//检测无敌
        LRMove();//角色左右移动
       if(currentHP<=0)
        {
            PlayerDeath();//角色死亡
        }
        //遍历动画函数
        //Attack2();//技能
        //检测无敌··
    }
    void PlayerDeath()//角色死亡
    {
        if(!playState.isDeath)//不是死亡的
        {
            playState.isDeath = true;//设成死亡
            anim.Play(deathAnimName);//播放死亡动画
            if(playState.rightSide)
            rigid.AddForce(new Vector2( -300f, 220f));
            else rigid.AddForce(new Vector2(300f, 220f));
            //enabled = false;//脚本不再触发
        }
    }
    void CheckUnmatched()//检测无敌··
    {
       
        if(playState.unmatchedTime>0.0f)
        {
            anim.SetFloat("changeColorTime", playState.unmatchedTime);
            playState.unmatchedTime -= Time.deltaTime;
            //playState.unmatchedTime -= Time.deltaTime;
            //float color = transform.GetComponent<SpriteRenderer>().color.r;//获取其中一个颜色
            //color=color > 50 ? (color-35) :255;//三目运算。小于150就255.不然就-2
            //Debug.Log(transform.GetComponent<SpriteRenderer>().color);
            //transform.GetComponent<SpriteRenderer>().color = new Color(color, color, color,255);//设置玩家颜色
            if (playState.unmatchedTime<=0.0f)
            {
                //transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);//如果无敌时间到了。就将颜色设回255
                playState.unmatchedTime = 0.0f;//后面是根据==0.0判断的
            }
        }
    }
    void prTime()
    {
        Debug.Log(Time.time);
    }
    void LRMove()//左右移动
    {
        if (IsName("run")&&playState.isGround)//如果播放的是跑步状态 并且是在地面
        {
            if (playState.leftArrow)
            {
                rigid.AddForce(new Vector2(-moveForce, 0));//给角色添加力
            }
            else if (playState.rightArrow)
            {
                rigid.AddForce(new Vector2(moveForce, 0));
            }
            if (Mathf.Abs(rigid.velocity.x) > moveMaxForce)//如果X轴力大于限制的速度。就设为最大速度
            {
                rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * moveMaxForce, rigid.velocity.y);
            }
        }

    }

    bool IsName(string name)//判断当前播放的是否某个动画名称
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    void Flip()//面向反转函数
    {
        if (IsName("idle") || IsName("run"))//如果是站立状态或跑步状态。才可以反面
        {
            Vector3 vt3 = transform.localScale;
            vt3.x *= -1;
            transform.localScale = vt3;//修改父物体x缩放为反方向
            playState.rightSide = !playState.rightSide;//设置角色面相相反
        }
    }
    void AbilityTrigger(string abilityName)//已弃用
    {
        colliderAssist.GetComponent<ColliderAssist>().TriggerCollider(abilityName);//开启碰撞，碰到的人将传进abilityName对应的类的函数里
    }
    //上下两个是一样的
    void FrameEvent(string abilityName)//帧触发接口
    {
        colliderAssist.GetComponent<ColliderAssist>().TriggerCollider(abilityName);//开启碰撞，碰到的人将传进abilityName对应的类的函数里
    }
}
