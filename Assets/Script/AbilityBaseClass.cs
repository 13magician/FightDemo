using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class AbilityBaseClass : MonoBehaviour {//所有技能的基类，含有技能常用的属性
    protected Animator anim;//给子类继承
    protected ActionState actState;//玩家的动作状态
    public abstract string AbilityName { get; set; }// //一定要设置类技能名称
    // Use this for initialization
    void Start()//占用子类Start函数，子类要使用Start函数应该调用AbiStart()函数。子类不要使用Start函数，不然会覆盖。不能正常初始化
    {
        Init();//给初始化子类的一些变量信息
        AbiStart();//给子类的Start接口
    }
    protected void Init() //一些变量初始化····
    {
        anim = GetComponent<Animator>();//获取角色的动画
       actState = GetComponent<ActionState>();//玩家的动作状态
    }
    protected virtual void AbiStart() { }//子类的Start函数（以后改下名字Init比较好点）
    /// <summary>
    /// 返回角色是否正在播放某个动画
    /// </summary>
    /// <param name="name">是否正在播放的动画名称</param>
    /// <returns></returns>
    protected bool IsName(string name)//返回角色是否正在播放某个动画
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    protected void AddForceX(float force)//给角色X添加力
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (actState.rightSide)
        {
            rigid.AddForce(new Vector2(force, 0));
        }
        else if (!actState.rightSide)//感觉这个判断有点多余
        {
            rigid.AddForce(new Vector2(-force, 0));
        }
    }
    protected void AddForceY(float force)//给角色Y添加力
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(0, force));
    }
    protected void AddKeyForceX(float force)//根据按键给角色添加力
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
    protected IEnumerator SetTrigger(string _animName, float wait = 0.8f)//让某个bool变成真，0.25秒后自动设回假
    {
        anim.SetBool(_animName, true);
        yield return new WaitForSeconds(wait);//等待0.25秒
        anim.SetBool(_animName, false);
    }
    protected IEnumerator SetTrigger(string _animName, string unlessTheAnim,float wait = 0.25f)//让某个bool变成真，0.25秒后自动设回假
    {
        bool exit = false;
        while(!exit)
        {
            yield return new WaitForSeconds(0.1f);//等待0.25秒
            if (IsName(unlessTheAnim))//除非正在播放这个动画，不然就设成假
                anim.SetBool(_animName, true);
            else
            {
                anim.SetBool(_animName, false);
                exit = !exit;//退出
            }
        }
    }
    /// <summary>
    /// 返回当前动画播放的时间比例
    /// </summary>
    /// <returns></returns>
    protected float GetAnimRate//返回当前动画播放的时间比例
    {
        get
        {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        }
    }
     /// <summary>
    /// 返回动画播放时间总长
    /// </summary>
    /// <returns></returns>
    protected float GetAnimLength//返回动画播放时间总长
    {
        get
        {
            return anim.GetCurrentAnimatorStateInfo(0).length;
        }
    }
    /// <summary>
    /// 返回动画还有多少秒播放完。不包括循环
    /// </summary>
    /// <returns></returns>
    protected float GetAnimEndTime//返回动画还有多少秒播放完。不包括循环
    {
        get
        {
            return GetAnimLength - GetAnimLength * GetAnimRate;
        }
    }
}
