using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AccidentPlayerCreep : MonoBehaviour
{
    float creepSpeed = 0.15f;//爬行速度
    float creepInterval = 0.2f;//爬行间隔
    public GameObject blueOrGreenLight;
    public AudioClip creepAudio;
    float deltaTime = 0f;
    int creepImgIndex = 0;
    public Sprite[] arrImg;
    [HideInInspector]
    public bool canWalk = false;
    [HideInInspector]
    public bool isShow = false;
    SpriteRenderer image;
	void Start () {
        image = GetComponent<SpriteRenderer>();
        image.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isShow)
        {
            image.enabled = true;
            blueOrGreenLight.SetActive(true);
        }
        if(!Input.GetKey(KeyCode.RightArrow)&&deltaTime>creepInterval)
        {
           // GetComponent<AudioSource>().PlayScheduled(2);
        }
	}
    void FixedUpdate()
    {
        deltaTime += Time.fixedDeltaTime;
        if (deltaTime>creepInterval&& canWalk && isShow)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (creepImgIndex >= arrImg.Length) creepImgIndex = 0;
                deltaTime = 0f;
                Creep();
            }
        }
    }
    void Creep()
    {
        transform.position += new Vector3(creepSpeed, 0f, 0f);
        image.sprite = arrImg[creepImgIndex];
        creepImgIndex++;
        GetComponent<AudioSource>().Play();
        //GetComponent<AudioSource>().PlayScheduled(2);
    }
}
