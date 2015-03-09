using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {//镜头跟踪角色

    public Transform character;//玩家角色
    public float xCameraMaxOffset = 1, yCameraMaxOffset = 1;//超过偏移镜头才会自动跟踪
    public float CamaeraMin = -16.37f, CamaeraMax = 4.36f;//限制镜头X轴的最大最小值
    public float xSmooth = 2f,ySmooth=2f;//设置镜头平移时的速度
    //private Transform playerCamera;//用于玩家的镜头
    private float cameraYoffset = 1.69f;//镜头Y轴和角色Y轴的偏移
   
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
        transform.position = new Vector3(character.position.x, character.position.y + cameraYoffset, transform.position.z);
        return;//用过度方式会出现类似卡顿现象
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
