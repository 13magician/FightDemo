using UnityEngine;
using System.Collections;

public abstract class ACTAbility : MonoBehaviour {//所有动作技能的基类
    protected Animator anim;//给子类继承
    public Player1 player;//玩家控制的角色··
    public abstract string AbilityName { get;  }// //一定要设置类技能名称
    void Start()//占用子类Start函数，子类要使用Start函数应该调用AbiStart()函数。子类不要使用Start函数，不然会覆盖。不能正常初始化
    {
        anim = GetComponent<Animator>();//获取角色的动画
        player = GetComponent<Player1>();//玩家
        if (!player.triggerAbility.ContainsKey(AbilityName))//如果玩家类的触发技能里没有我的技能
        {
            player.triggerAbility.Add(AbilityName, TriggerAbility);//给玩家添加技能接口
        }
        Init();//给初始化子类的一些变量信息
    }
    protected virtual void TriggerAbility(Transform hit) { }//技能碰撞的接口。以后可以直接使用了
    protected virtual void Init() { }//一些变量初始化····
    // Update is called once per frame
    void Update () {
	
	}
    protected bool IsName(string name)//返回角色是否正在播放某个动画
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    protected void SetTrigger(string _animName, float wait = 0.25f)
    {
        StartCoroutine(IESetTrigger(_animName, wait));
    }
    protected IEnumerator IESetTrigger(string _animName, float wait = 0.25f)//让某个bool变成真，0.25秒后自动设回假
    {
        anim.SetBool(_animName, true);
        yield return new WaitForSeconds(wait);//等待0.25秒
        anim.SetBool(_animName, false);
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
    protected void CheckEffectSide(Transform bindObj, GameObject effect)
    {
        if (transform.position.x > bindObj.position.x)//如果玩家在怪物右边。就变换特效的缩放
        {
            effect.transform.localScale = new Vector2(-1 * effect.transform.localScale.x, effect.transform.localScale.y);//变换特效的缩放···名字有点长
        }
    }
}
