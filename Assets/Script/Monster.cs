using UnityEngine;
using System.Collections;

public abstract class Monster : MonoBehaviour {//所有怪物的基类

    public abstract void wasAttacked(float wasAttackedDurationTime);//所有怪物都应该有被攻击函数
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
