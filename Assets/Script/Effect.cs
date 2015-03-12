using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {//给物体添加绑定的特效（就是不断移动特效到物体）

    // Use this for initialization
   // public static Transform effect;//特效物体
    public  Animator anim;
    public Transform bindObj;//移动绑定的对象
    //public Vector3 effectOffset;//特效偏移
    public float duration = 0.0f;
    //Vector3 defScale;//保存默认尺寸
   void Awake()
    {
        //因为动画创建立马要用，所以放这里
        anim = GetComponent<Animator>();//获取特效动画，这才是显示特效关键
    //    defScale = transform.localScale;//保存默认尺寸
    }
	void Start () {
        //  effect = GameObject.FindGameObjectWithTag("effect").transform;//寻找特效
        check();
    }
	void Update()
    {
        //StartCoroutine( check());//等一这一针结束
        check();
    }
    void check()
    {
       // yield return new WaitForEndOfFrame();//等一这一针结束
        if (bindObj != null && duration > 0)//绑定的对象不是空以及持续时间要大于0
        {
            transform.position = bindObj.position + bindObj.GetComponent<Monster>().BindEffectOffset1;//获取绑定位置的更新
            duration -= 0.02f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //需要在Monster类里弄个Get特效位置
    public void bindEffect(Transform _bindObj,float durationTime,string animName)//绑定特效
    {
        bindObj = _bindObj;
       // effectOffset = _bindObj.GetComponent<Monster>().BindEffectOffset1;
        if (durationTime != 0.0f) duration = durationTime;//如果传进来的时间是0.就给他float最大值
        else duration = float.MaxValue;
        anim.Play(animName);
    }
    void Delete()//帧删除接口
    {
        Destroy(gameObject);
    }
}
