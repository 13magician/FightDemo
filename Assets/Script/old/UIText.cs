using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIText : MonoBehaviour {

    // Use this for initialization
    public PlayerControl player;
    Text text;
    string startText;
	void Start () {
        text = GetComponent<Text>();
        startText = text.text;
	}
	
	// Update is called once per frame
	void Update () {
        text.fontSize = Screen.height / 20;
        if(Time.time<15f)
            text.text = startText + player.killGreenWaterNum+"    ←→移动，X攻击，→+Z突刺";
        else text.text = startText + player.killGreenWaterNum;
        //Debug.Log(Screen.width + ":" + Screen.height);
    }
}
