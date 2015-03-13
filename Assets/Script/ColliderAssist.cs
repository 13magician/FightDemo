using UnityEngine;
using System.Collections;

public class ColliderAssist : MonoBehaviour {
    PlayerControl ply;//玩家的脚本
    string abilityName;
    // Use this for initialization
    void Start () {
        GetComponent<Collider2D>().enabled = false;//初始化让他false····就不用手动调了
        ply = transform.parent.GetComponent<PlayerControl>();//玩家的脚本
    }
	
	// Update is called once per frame
	void Update () {
    }
    void OnTriggerEnter2D(Collider2D hit)//碰撞打算用这个。多个物体可以正常接受
    {
        //Debug.Log("碰撞触发前:" + Time.time);//设为true，立马同一时间就能检测
        if (ply.triggerAbility.ContainsKey(abilityName))//先看看有没有这个key
        ply.triggerAbility[abilityName](hit.transform);//将碰到的人传进指定的技能类的函数里
       // Debug.Log("碰撞触发后:" + Time.time);//跟上面那个时间一样。说明很快
  
    }
    /*协同
    yield return null;        //让出时间片，让父进程先执行
    yield return new WaitForSeconds(1);            //在程序执行到1秒时才执行接下来的代码
    yield return new WaitForFixedUpdate();        //在FixedUpdate调用之后才执行接下来的代码
    yield return new WaitForEndOfFrame();        //在这一帧结束之后才执行接下来的代码
    yield return new StartCoroutine("xxx");        //在xxx协同执行完之后才执行接下来的代码
        */
    IEnumerator CloseCollider()
    {
       // Debug.Log("结束前:" + Time.time);//也是立马调用，继续是同一时间
        yield return new WaitForEndOfFrame();//等待当前帧执行完毕
        GetComponent<Collider2D>().enabled = false;//协作程序，功效是这帧完成后关闭碰撞激活。
       //  Debug.Log("结束后：" + Time.time);//这里等待了0.00704秒
    }
    void OnTriggerExit2D(Collider2D hit)//退出时使用  好像没机会退出了- -
    {
    
    }
    void OnTriggerStay2D(Collider2D hit)//在这里关闭碰撞。每帧发送。在所有的OnTriggerEnter2D后再才会处理这个。
    {
        GetComponent<Collider2D>().enabled = false;//关闭
    }
    public void TriggerCollider(string name)//碰撞接口。传的是技能名称
    {
        abilityName = name;//记录触发技能名称
        GetComponent<Collider2D>().enabled = true;//开启碰撞
        StartCoroutine(CloseCollider());//协作程序，功效是这帧完成后关闭碰撞激活。也是立马调用，继续是同一时间
        // Debug.Log("开启碰撞:" + Time.time);
    }

}
