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
    [HideInInspector]
    public bool isGround = false;//是否在地面。可以放到基类里··这个考虑是否放在怪物基类
    public Transform groundCheck;//地面检测辅助对象
    void Start () {//在基类重定义吧··
        anim = GetComponent<Animator>();
    }
    public override void wasAttacked(float wasAttackedDurationTime)
    {
        wasAttackedEndTime = wasAttackedDurationTime;//设置被攻击持续时间
    }
    void Update()
    {
        wasAttackedAnim();//调用被攻击处理动画
        isGround = Physics2D.Linecast(groundCheck.position, transform.position, 1 << LayerMask.NameToLayer("ground"));//检测是否在地面
        // bindEffectOffset1 =  new Vector3(0, transform.GetComponent<SpriteRenderer>().sprite.rect.position.y * 0.5f*transform.localScale.y, 0);//计算设置特效偏移位置。
        bindEffectOffset1 = new Vector3(0, 0.7f * 0.5f * transform.localScale.y, 0);//计算设置特效偏移位置。
        Debug.Log(bindEffectOffset1);
    }
    void OnCollisionEnter2D(Collision2D hit)  //碰撞进入``` 玩家被怪物碰到
    {
        CheckCollision(hit);//交给别人处理
    }
    void OnCollisionStay2D(Collision2D hit)//碰撞持续
    {
        CheckCollision(hit);
    }
    void CheckCollision(Collision2D hit)//处理怪物I碰到玩家
    {
        if (hit.transform.tag == "Player" && hit.transform.name == "hero" && hit.gameObject.GetComponent<PlayerControl>() != null)//碰到的是玩家··不用那么多判断
        {
            Transform hitTf = hit.transform;
            if (hitTf.GetComponent<ActionState>().unmatchedTime == 0.0f)//条件是没有无敌持续时间
            {
                hitTf.GetComponent<ActionState>().unmatchedTime = 0.8f;//给玩家添加无敌时间
                hitTf.GetComponent<PlayerControl>().anim.Play("wasAttacked");//播放被攻击动画
                Rigidbody2D rigid = hitTf.GetComponent<Rigidbody2D>();
                float xVelocity = 8f, yVelocity = 4.5f;//被碰到撞飞的速度
                if (hitTf.position.x < transform.position.x)//如果怪物在玩家右边
                {
                    xVelocity *= -1;
                    yVelocity *= -1;
                }
                if (rigid.velocity.x < xVelocity)//如果X速度没达到要求
                {
                    rigid.velocity = new Vector2(xVelocity, rigid.velocity.y);//设置速度
                }
                if (rigid.velocity.y < yVelocity)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, yVelocity);
                }
            }
        }
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
        if(isGround&&anim.speed==0.015f)//如果在地面，并且动画播放速度是0.015。就让他设回1
        {
            anim.speed = 1;
        }
        if (IsName("idle_greenWater")&&isGround)//是空闲状态。以及在地面
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
                AddForce(jumpForceX, jumpForceY);//考虑放到基类
            }
            MoveMaxSpeed(maxSpeedX, maxSpeedY);//限制最大移动速率··考虑放到基类
        }
    }
    void SetAnim0015()//将自身动画播放速度为0.015
    {
        anim.speed = 0.015f;
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
