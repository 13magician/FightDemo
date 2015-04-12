using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PrologueText : MonoBehaviour {
    public float scrollSpeed = 10f;
    RectTransform rectTransform;
    bool isTextEnd = false;
    public PrologueBlackBG blackBG;
    public PrologueCar car;
    float textEndTime = 25f;
	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().fontSize = Screen.width / 40;
    }
    void FixedUpdate()
    {
        rectTransform.anchoredPosition +=new Vector2(0, Screen.width*scrollSpeed);
        Debug.Log(Time.fixedTime);
        if(Time.fixedTime>18f)
        {
            car.carSpeed += 0.0004f;
            car.GetComponent<Animator>().speed += 0.001f;
        }
       if(Time.fixedTime>textEndTime&&isTextEnd==false)
       {
           // 画面开始变暗
           isTextEnd = true;
           blackBG.PlayBlackBG();
       }
    }

}
