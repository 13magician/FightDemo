using UnityEngine;
using System.Collections;
using System;

public class Assault : AbilityBaseClass {

    private string abilityName="Assault";//一定要设置类技能名称
    public string animName1 = "assault";
    public float assaultForceX = 30f, assaultForceY = 10f;
    public float assaultSpeed = 2, assaultMaxSpeed=15f;
    // Use this for initialization
    protected override void AbiStart()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && IsName("run"))
        {
            StartCoroutine(SetTrigger(animName1));//设置真，0.25秒后变成假
        }
    }
    void FixedUpdate()
    {
        if(IsName(animName1))
        {
            //AddForceX(assaultForceX);
            // GetComponent<Rigidbody2D>().velocity = new Vector2(assaultSpeed, 1f);
            AddForceY(assaultForceY);
        }
    }
    public override string AbilityName
    {
        get
        {
            return abilityName;   
        }

        set
        {
            abilityName = value;
        }
    }
}
