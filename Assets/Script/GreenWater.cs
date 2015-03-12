using UnityEngine;
using System.Collections;

public class GreenWater : Monster {//绿水灵的脚本。我想在动画里设置跳起时候的播放速度0.接触地面才改回正常

    // Use this for initialization
    Animator anim;//这个可以放到怪物基类
    bool rightSide = true;//面相 可以放到怪物基类
    float maxRnd =  3 *50;//3秒的几率跳
   public float jumpForceX = 10f,jumpForceY=20f;//怪物跳起来的力
    float maxSpeedX = 1.65f, maxSpeedY = 1.65f;//限制怪物的最大速度
    float wasAttackedEndTime = 0.0f;//距离结束播放被攻击动画还剩多少秒。绑定特效持续时间
    bool isGround = false;//是否在地面。可以放到基类里
    void Start () {//在基类重定义吧··
        anim = GetComponent<Animator>();
    }
    public override void wasAttacked(float wasAttackedDurationTime)
    {
        wasAttackedEndTime = wasAttackedDurationTime;//设置被攻击持续时间
    }
    //public void bindEffect(GameObject effect,float durationTime)//绑定特效接口，特效。被攻击动画持续时间。
    //{
    //    effectDuration = durationTime;//设置被攻击持续时间
    //}
    // Update is called once per frame
    void Update () {
        wasAttackedAnim();//调用被攻击处理动画
        float yMax = transform.GetComponent<SpriteRenderer>().sprite.rect.position.y;
        //  Debug.Log(yMax - yMin);
        bindEffectOffset1 =  new Vector3(0,0.7f*0.5f*transform.localScale.y, 0);//设置特效偏移位置
      //Debug.Log(transform.GetComponent<SpriteRenderer>().sprite.rect.yMax);
    }
    void wasAttackedAnim()//被攻击处理动画
    {
        if (wasAttackedEndTime > 0 && !IsName("wasAttacked"))//如果结束被攻击动画时间大于0，且不在被攻击动画状态
        {
            anim.SetBool("wasAttacked", true);//设成被攻击
            wasAttackedEndTime -= Time.deltaTime;//一个不精确但是可以用的处理被攻击动画方法··
            if(wasAttackedEndTime<=0)
            {
                wasAttackedEndTime = 0;
                anim.SetBool("wasAttacked", false);
            }
        }
    
    }
    public override Vector3 BindEffectOffset1//所有怪物类都应该有绑定特效偏移
    {
        get
        {
            return bindEffectOffset1;
        }

        set
        {
            bindEffectOffset1 = value;
        }
    }
    void FixedUpdate()
    {
        if (IsName("idle_greenWater"))//是空闲状态
        {
          if( Random.Range(0f,maxRnd)<1)//3秒几率会触发移动
            {
                if (Random.Range(0, 5)==0) Flip();//4次有一次几率改变面相
                anim.SetTrigger("action");//播放跳动动画
            }
        } 
        else if(IsName ("action_greenWater"))//如果是蹦跳动画
        {
            if(GetAnimRate>0.16f && GetAnimRate<0.3f)//准备起跳和跳跃
            {
                AddForce(jumpForceX, jumpForceY);
            }
            MoveMaxSpeed(maxSpeedX, maxSpeedY);//限制最大移动速率
        }
    }
    bool IsName(string name)//判断当前播放的是否某个动画名称-最高基类
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    void Flip()//面向反转函数-最高基类
    {
        Vector3 vt3 = transform.localScale;
        vt3.x *= -1;
        transform.localScale = vt3;//修改父物体x缩放为反方向
        rightSide = !rightSide;//设置角色面相相反
    }
    protected float GetAnimRate//返回当前动画播放的时间比例-最高基类
    {
        get
        {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        }
    }
    protected float GetAnimLength//返回动画播放时间总长-最高基类
    {
        get
        {
            return anim.GetCurrentAnimatorStateInfo(0).length;
        }
    }
    void AddForce(float force)//给角色添加力--怪物基类
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (rightSide)
        {
            rigid.AddForce(new Vector2(force, 0));
        }
        else if (!rightSide)
        {
            rigid.AddForce(new Vector2(-force, 0));
        }
    }
    void AddForce(float forcex,float forcey)//给角色添加力--怪物基类，角色也要重置
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (rightSide)
        {
            rigid.AddForce(new Vector2(forcex, forcey));
        }
        else if (!rightSide)
        {
            rigid.AddForce(new Vector2(-forcex, forcey));
        }
    }
    void MoveMaxSpeed(float moveMaxSpeedX,float moveMaxSpeedY)//限制移动最大速度，怪物角色共同基类
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (Mathf.Abs(rigid.velocity.x) > moveMaxSpeedX)//如果X轴力大于限制的速度。就设为最大速度
        {
            rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * moveMaxSpeedX, rigid.velocity.y);
        }
        //Y轴
        if (Mathf.Abs(rigid.velocity.y) > moveMaxSpeedY)//如果Y轴力大于限制的速度。就设为最大速度
        {
            rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Sign(rigid.velocity.y) * moveMaxSpeedY);
        }
    }
}
