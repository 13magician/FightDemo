using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class Player1 : MonoBehaviour {

    //public:
    public float xWalkSpeed = 0.05f, yWalkSpeed=0.03f,xRunSpeed=0.5f,yRunSpeed=0.35f;//玩家走跑速度
    public Image HPBar;//血条
    public float jumpSpeed=5f,jumpXquiken=0.1f,jumpYquiken=-0.25f;//跳跃时的力、速度。跳跃时的X轴加速比率
    public float countHP = 50f, currentHP = 50f;//主角的血量
    public float runKeyInterval = 0.3f;//跑按键间隔
    public Transform colliderAssist,groundCheck;//攻击辅助碰撞脚本对象，地面检测
    public delegate void TriggerAbility(Transform hit);//定义一个委托，用于触发技能。主要给下面的Dictionary用
    public Dictionary<string, TriggerAbility> triggerAbility = new Dictionary<string, TriggerAbility>();//定义一个Dictionary(Mapping)。映射相应的技能
    //HideInInspector:
    [HideInInspector]//属性不显示在Inspector。虽然不显示，但是仍然会取属性不显示在Inspector面板中的默认值···记得去掉查看/修改
    public Animator anim;
    [HideInInspector]//隐藏在InInspector面板显示
    public bool isRightSide = true;//控制角色的面相,.默认是右边
    [HideInInspector]
    public Transform root;
    [HideInInspector]
    public bool isGround;//是否在地面···
    [HideInInspector]
    public bool isGroundNormalAction = false;//是否能在地面上正常行动。玩家需要在地面上，不被攻击
    [HideInInspector]
    public bool isSkyNormalAction = false;//是否能在空中正常移动。玩家需要在空中，不被攻击
    [HideInInspector]
    public bool isDeath = false;//是否死亡
    [HideInInspector]
    public bool rightArrow, leftArrow, upArrow, downArrow;//玩家是否按上下左右键
    [HideInInspector]
    public int killGreenWaterNum = 0;//杀死绿水灵的数量
    [HideInInspector]
    public float unmatchedTime = 0.0f;//无敌时间
    [HideInInspector]
    public Rigidbody2D rigid;//角色的刚体组件
    //private:
    private string deathName = "death", walkName = "walk", runName = "run", idleName = "idle", jumpName = "jump";//死亡、走、跑的动画名称
    private string deathVar = "death", walkVar = "walk", runVar = "run", idleVar = "idle", jumpVar = "jump";//控制死亡、走、跑的变量名称
    private string changeColorTimeVarName = "changeColorTime";//改变颜色变量名的值
    private readonly object playState;
    private KeyCode jumpKey = KeyCode.Space;//设置跳跃键
    KeyCode upKey = KeyCode.UpArrow,downKey=KeyCode.DownArrow,leftKey=KeyCode.LeftArrow,rightKey=KeyCode.RightArrow;//设置上下左右键
    private float lastDownTime;//第一次按下左右的时间
    private bool lastRightSide;//按下左右时的面相
    private bool isRunJump;//记录跳跃前是行走还是跑
    //public Transform mainCastAssist;//辅助投射的父物体
    //public  Transform[] castAssist;//4个辅助投射
    //public Transform mainColliderAssist;//攻击辅助碰撞


    void Start()
    {
        anim = GetComponent<Animator>();
        root = transform.root;//根物体
        rigid =root.GetComponent<Rigidbody2D>();//根物体的刚体

        //添加技能组件
        //gameObject.AddComponent<Attack1>();
        //gameObject.AddComponent<Assault>();
        gameObject.AddComponent<Zhiquan>();
    }

    // Update is called once per frame
    void Update()
    {
        leftArrow = upArrow = downArrow = rightArrow = false;//默认是false··无视这点效率了
        if (leftArrow = Input.GetKey(leftKey)) { }//是否按下左右上下键
        else rightArrow = Input.GetKey(rightKey);//否则处理右键
        if (upArrow = Input.GetKey(upKey)) { }//如果有按下上键
        else downArrow = Input.GetKey(downKey);//否则处理下键
        if (rightArrow && !isRightSide) Flip();//如果玩家按下右方向，并且面相不是右边。调用反转函数
        else if (leftArrow && isRightSide) Flip();//否则判断如果玩家按下←方向，并且面相是右边。调用反转函数
        if(Input.GetKeyDown(leftKey) || Input.GetKeyDown(rightKey) && isGroundNormalAction)//跑
        {
            bool downRight = Input.GetKeyDown(rightKey), downLeft = Input.GetKeyDown(leftKey);//重写获取是否按下左右键
            if (downRight && !isRightSide) Flip();//如果玩家按下右方向，并且面相不是右边。调用反转函数
            if (downLeft && isRightSide) Flip();////否则判断如果玩家按下←方向，并且面相是右边。调用反转函数
            if (Time.time-lastDownTime<= runKeyInterval && lastRightSide==isRightSide)//如果距离上一次按下的时间小于0.5秒。以及面向跟上一次的一样
            {
                anim.SetBool(runVar, true);//设置跑
            }
            lastDownTime = Time.time;//上一次按下的时间
            lastRightSide = isRightSide;//上一次按下时的面向
        }
        if (isGroundNormalAction) anim.SetBool(walkVar, rightArrow || leftArrow || upArrow || downArrow);//判断玩家是否在地面正常行动。按下左右任意一个都true
        if(isGroundNormalAction&& anim.GetBool(runVar)) anim.SetBool(runVar, rightArrow || leftArrow || upArrow || downArrow);
        CheckJump();//检查跳跃
        //上面是行走功能
        //HPBar.fillAmount = Mathf.Lerp(HPBar.fillAmount, currentHP / countHP, Time.deltaTime * countHP * 0.05f + 0.05f);//设置UI的血量显示
    }
    void FixedUpdate()
    {
        if (currentHP <= 0)
        {
            PlayerDeath();//角色死亡
        }
        CheckUnmatched();//检测无敌
        isGround = Physics2D.Linecast(groundCheck.position, root.position, 1 << LayerMask.NameToLayer("ground"));//检测是否在地面
        CheckIsSkyNormalAction();//检测是否能在空中正常行动
        CheckIsGroundNormalAction();//检测是否能在地面正常行动
        LRwalk();//角色左右走
        UDwalk();//角色上下走
        LRrun();//角色左右跑
        UDrun();//角色上下跑
        SkyLRwalk();//空中左右行走
        SkyUDwalk();//空中上下行走
        SkyLRrun();//空中左右跑
        SkyUDrun();//空中上下跑
        //遍历动画函数
        //Attack2();//技能
        //检测无敌··
    }
    void CheckJump()
    {
        if(Input.GetKeyDown(jumpKey)&&!IsName(jumpName))//按下跳跃键以及不是正在播放跳跃动画
        {
            if(isGroundNormalAction)//如果能在地面正常活动
            {
                anim.SetBool(jumpVar, true);//设置跳跃变量为true
                //rigid.AddForce(new Vector2(0, 200f));
                rigid.velocity = new Vector2(rigid.velocity.x, jumpSpeed);//直接设置速度
                isRunJump = false;//记录跳跃前是行走还是跑
                if (IsName(runName))//如果是跑动画
                {
                    isRunJump = true;
                }
            }
        }
        else
        {
            anim.SetBool(jumpVar, false);//如果没按下jump键就让他成为false
        }
 //       if (Input.GetKeyDown(jumpKey) && isGround && !IsName(jumpName))
    }

    void LRwalk()//左右行走
    {
        if (IsName(walkName) && isGroundNormalAction)//如果播放的是行走状态 并且是在地面
        {
            if (leftArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x - xWalkSpeed, v.y, v.z);//左右移动是改变根的位置
                //rigid.AddForce(new Vector2(-moveForce, 0));//给角色添加力
            }
            else if (rightArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x + xWalkSpeed, v.y, v.z);//左右移动是改变根的位置
            }
        }
    }
    void SkyLRwalk()//空中左右行走
    {
        if (isSkyNormalAction&&!isRunJump)// && IsName(jumpName))//在空中以及不是跑着跳
        {
            if(leftArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x - xWalkSpeed * (1 + jumpXquiken), v.y, v.z);
            }
            else if(rightArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x + xWalkSpeed * (1 + jumpXquiken), v.y, v.z);
            }
        }
    }
    void UDwalk()//上下行走
    {
        if (IsName(walkName) && isGroundNormalAction)//如果播放的是行走状态 并且是在地面
        {
            if (upArrow)
            {
                Vector3 v = transform.position;
               transform.position = new Vector3(v.x , v.y+yWalkSpeed , v.z);//上下移动是改变自身的位置
                //rigid.AddForce(new Vector2(-moveForce, 0));//给角色添加力
            }
            else if (downArrow)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y - yWalkSpeed, v.z);//上下移动是改变自身的位置
            }
        }
    }
    void SkyUDwalk()//空中上下行走
    {
        if (isSkyNormalAction&&!isRunJump)//&& IsName(jumpName))//空中正常活动，以及是跑的时候跳的
        {
            if (upArrow)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y + yWalkSpeed * (1 + jumpYquiken), v.z);
            }
            else if (downArrow)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y - yWalkSpeed * (1 + jumpYquiken), v.z);
            }
        }
    }
    void LRrun()//左右跑
    {
        if (IsName(runName) && isGroundNormalAction)//如果播放的是run状态 并且是在地面
        {
            if (leftArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x - xRunSpeed, v.y, v.z);//左右移动是改变根的位置
                //rigid.AddForce(new Vector2(-moveForce, 0));//给角色添加力
            }
            else if (rightArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x + xRunSpeed, v.y, v.z);//左右移动是改变根的位置
            }
        }
    }
    void SkyLRrun()//空中左右跑
    {
        if(isSkyNormalAction&&isRunJump)//&& IsName(jumpName))//以及跑着跳的
        {
            if (leftArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x - xRunSpeed*(1+jumpXquiken), v.y, v.z);//跳跃加速
            }
            else if (rightArrow)
            {
                Vector3 v = root.position;
                root.position = new Vector3(v.x + xRunSpeed * (1 + jumpXquiken), v.y, v.z);
            }
        }
    }
    void UDrun()//上下跑
    {
        if (IsName(runName) && isGroundNormalAction)//如果播放的是行走状态 并且是在地面
        {
            if (upArrow)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y + yRunSpeed, v.z);//上下移动是改变自身的位置
                //rigid.AddForce(new Vector2(-moveForce, 0));//给角色添加力
            }
            else if (downArrow)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y - yRunSpeed, v.z);//上下移动是改变自身的位置
            }
        }
    }
    void SkyUDrun()//空中上下跑
    {
        if(isSkyNormalAction&&isRunJump)// && IsName(jumpName))//如果空中活动以及跑着跳的
        {
            if (upArrow)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y +yRunSpeed *(1 + jumpYquiken), v.z);
            }
            else if (downArrow)
            {
                Vector3 v = transform.position;
                transform.position = new Vector3(v.x, v.y - yRunSpeed * (1 + jumpYquiken), v.z);
            }
        }
    }

    void CheckIsSkyNormalAction()//检测是否能在空中正常行动
    {
        isSkyNormalAction = false;
        if (!isGround&&!isDeath)//如果不是在地面和不是死亡
        {
            isSkyNormalAction = true;
        }
    }
    void CheckIsGroundNormalAction()//检测是否能在地面正常行动
    {
        isGroundNormalAction = false;
        if (isGround && !isDeath && (IsName(idleName) || IsName(walkName) || IsName(runName)))//如果是在地面和不是死亡。以及走、跑、站立任意一种状态
        {
            isGroundNormalAction = true;
        }
    }
    void PlayerDeath()//角色死亡
    {
        if (!isDeath)//不是死亡的
        {
            isDeath = true;//设成死亡
            anim.Play(deathName);//播放死亡动画
            if (isRightSide)
                rigid.AddForce(new Vector2(-300f, 220f));
            else rigid.AddForce(new Vector2(300f, 220f));
            //enabled = false;//脚本不再触发
        }
    }
    void CheckUnmatched()//检测无敌··
    {
        if (unmatchedTime > 0.0f)
        {
            anim.SetFloat(changeColorTimeVarName, unmatchedTime);//设置改变颜色变量名的值
            unmatchedTime -= Time.deltaTime;
            //playState.unmatchedTime -= Time.deltaTime;
            //float color = transform.GetComponent<SpriteRenderer>().color.r;//获取其中一个颜色
            //color=color > 50 ? (color-35) :255;//三目运算。小于150就255.不然就-2
            //Debug.Log(transform.GetComponent<SpriteRenderer>().color);
            //transform.GetComponent<SpriteRenderer>().color = new Color(color, color, color,255);//设置玩家颜色
        }
        unmatchedTime = Mathf.Max(unmatchedTime, 0);//尽量不然无敌时间小于0
    }


    bool IsName(string name)//判断当前播放的是否某个动画名称
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    void Flip()//面向反转函数
    {
        if (isGroundNormalAction||isSkyNormalAction)//如果是在地面能正常活动状态。才能反转
        {
            Vector3 vt3 = transform.localScale;
            vt3.x *= -1;
            transform.localScale = vt3;//修改父物体x缩放为反方向
            isRightSide = !isRightSide;//设置角色面相相反
        }
    }
    void AbilityTrigger(string abilityName)//帧触发接口/
    {
        colliderAssist.GetComponent<ColliderAssist>().TriggerCollider(abilityName);//开启碰撞，碰到的人将传进abilityName对应的类的函数里
    }
    //上下两个是一样的
    void FrameEvent(string abilityName)//已弃用
    {
        colliderAssist.GetComponent<ColliderAssist>().TriggerCollider(abilityName);//开启碰撞，碰到的人将传进abilityName对应的类的函数里
    }
}
