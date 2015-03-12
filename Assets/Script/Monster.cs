using UnityEngine;
using System.Collections;

public abstract class Monster : MonoBehaviour {//所有怪物的基类

    public abstract void wasAttacked(float wasAttackedDurationTime);//所有怪物都应该有被攻击函数
    public abstract Vector3 BindEffectOffset1 { get; set; }//所有怪物都应该有绑定特效偏移
    protected Vector3 bindEffectOffset1;
    [HideInInspector]
    public int HP = 99999;//怪物的血量，可以考虑放到基类
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
