using UnityEngine;
using System.Collections;

public class PrologueCar : MonoBehaviour {

    Animator anim;
    public Transform endGasPosition;
    public float carSpeed = 2.0f;
    GameObject endGasParticle;//尾气特效

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        endGasParticle = Resources.Load("Prologue/EndGasParticl") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        MoveCar();
    }
    /// <summary>
    /// 让车移动
    /// </summary>
    void MoveCar()
    {
        Vector3 scurrentPosition = transform.position;
        transform.position = transform.position + new Vector3(carSpeed, 0, 0);
    }
    /// <summary>
    /// 让尾气管播放气体粒子效果
    /// </summary>
    void PlayParticle()
    {
        GameObject endGas = Instantiate<GameObject>(endGasParticle);
        endGas.transform.position = endGasPosition.position;
        Destroy(endGas, 5f);
    }
}
