using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {//镜头跟踪角色

    public Transform play;//玩家角色
    public float xCameraMaxOffset = 1f, yCameraMaxOffset = 0.1f;//超过偏移镜头才会自动跟踪
    public float xCameraMin = -13.07199f, xCameraMax = 1.088597f;//限制镜头X轴的最大最小值
    public float yCameraMin = 0.1f;
    public float xSmooth = 1.5f, ySmooth= 0.5f;//设置镜头平移时的速度
    //private Transform playerCamera;//用于玩家的镜头
    public float cameraYoffset = -1.582179f;//镜头Y轴和角色Y轴的偏移
   
    void Start () {
        // playerCamera = (GameObject.FindWithTag("MainCamera") as GameObject).GetComponent<Transform>();//获取MainCamera摄像机的transform
        //character = (GameObject.FindWithTag("Player") as GameObject).GetComponent<Transform>();//获取玩家角色
    }
	
	// Update is called once per frame
	void Update () {
      
    }
    void LateUpdate()
    {
        ResetCamera();//重设镜头
    }
    void ResetCamera()//设置镜头的XY跟角色对齐
    {
        Vector3 t = transform.position;//镜头的位置
        Vector3 p = play.position;//玩家的位置
       if(Mathf.Abs(p.x-t.x)>xSmooth)//如果镜头的X轴超过玩家一定范围····
        {
            float x = p.x + (Mathf.Sign(p.x - t.x) * xSmooth * -1);//X轴最大最小位置
            if (x > 0) x = Mathf.Min(x, xCameraMax);
            else x = Mathf.Max(x, xCameraMin);
            transform.position = new Vector3(x, t.y,t.z);
            t = transform.position;//镜头的位置改变了，要重新赋值
        }
       //Y轴的偏移上下是不一致的，以后重写
        if (Mathf.Abs(p.y - (t.y)) > ySmooth)//如果镜头的Y轴超过玩家一定范围····
        {
            float y = Mathf.Lerp(t.y, p.y + (Mathf.Sign(p.y - t.y) * ySmooth * -1f), Time.deltaTime);//插值
           // y += cameraYoffset;
            transform.position = new Vector3(t.x,Mathf.Max(yCameraMin, y) ,t.z);//Y轴最小不能小于yCameraMin
        }
        return;
        //if(play.position.x-transform.position.x >xSmooth)
        //{
        //    Vector3 v3 = play.position;
        //    transform.position = new Vector3(v3.x - xSmooth, v3.y + cameraYoffset, transform.position.z);
        //}
        //if (play.position.x - transform.position.x < xSmooth)
        //{
        //    Vector3 v3 = play.position;
        //    transform.position = new Vector3(v3.x + xSmooth, v3.y+ cameraYoffset, transform.position.z);
        //}


        //transform.position = new Vector3(Mathf.Clamp(play.position.x,CamaeraMin, CamaeraMax), play.position.y + cameraYoffset, transform.position.z);
        //return;//用过度方式会出现类似卡顿现象
        //Vector3 vt3 = transform.position;//默认保存镜头的向量
        //if (Mathf.Abs(character.position.x - transform.position.x) > xCameraMaxOffset)//玩家的X位置减去镜头所在的Y位置大于镜头跟随最大值
        //{
        //    vt3.x = character.position.x;//获取玩家的X坐标
        //    vt3.x = Mathf.Lerp(transform.position.x, vt3.x, Time.deltaTime*xSmooth);//设置镜头的X值
        //}
        //if (Mathf.Abs(character.position.y - transform.position.y) > yCameraMaxOffset)//玩家的Y位置减去镜头所在的Y位置大于镜头跟随最大值
        //{
        //    vt3.y = character.position.y + cameraYoffset;//计算镜头和角色Y轴偏移
        //    vt3.y = Mathf.Lerp(transform.position.y, vt3.y, Time.deltaTime * ySmooth);//设置镜头的Y值
        //}
        //transform.position = vt3;//最终改变镜头的位置
    }
}
