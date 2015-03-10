using UnityEngine;
using System.Collections;

public class AbilityBaseClass : MonoBehaviour {//所有技能的基类，含有技能常用的属性
    protected Animator anim;//给子类继承
	// Use this for initialization
       void Start () {//占用子类Start函数，子类要使用Start函数应该调用AbiStart()函数。子类不要使用Start函数，不然会覆盖。不能正常初始化
        Init();//给初始化子类的一些变量信息
        AbiStart();//给子类的Start接口
	}
    protected void Init() //一些变量初始化····
    {
        anim = GetComponent<Animator>();//获取角色的动画
    }
    protected virtual void AbiStart() { }//子类的Start函数
    protected bool IsName(string name)//返回角色是否正在播放某个动画
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
