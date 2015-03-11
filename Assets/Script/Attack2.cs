using UnityEngine;
using System.Collections;
using System;

public class Attack2 : AbilityBaseClass//继承AbilityBaseClass的一些常用属性
{

    // Use this for initialization
    public override string AbilityName
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.X) && !IsName("attack2"))//发动攻击2动画
        //{
        //    anim.SetTrigger("attack2");
        //}
    }
}
