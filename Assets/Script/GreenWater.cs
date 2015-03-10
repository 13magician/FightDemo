using UnityEngine;
using System.Collections;

public class GreenWater : MonoBehaviour {

    // Use this for initialization
    Animator anim;
    bool rightSide = true;//面相
    float maxRnd =  3 *50;//3秒的几率跳
   public float jumpForceX = 10f,jumpForceY=20f;//怪物跳起来的力
    float maxSpeedX = 1.65f, maxSpeedY = 1.65f;//限制怪物的最大速度
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

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
