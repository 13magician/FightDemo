using UnityEngine;
using System.Collections;
using System;

public class Zhiquan : ACTAbility
{//继承动作技能

    private string abilityName = "Zhiquan";//技能名称
    public KeyCode TriggerKey = KeyCode.X;//触发技能按键
    private string animName1 = "zhiquan";//播放动作的名称
    private string animVar1 = "zhiquan";
    public override string AbilityName
    {
        get
        {
            return abilityName;
        }
    }
    // Use this for initialization
    protected override void Init()
    {

    }
    protected override void TriggerAbility(Transform hit)//技能触发接口
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(TriggerKey))
        {
            if(player.isGroundNormalAction)
            {
                SetTrigger(animName1, 0.25f);
            }
        }
    }
}
