using UnityEngine;
using System.Collections;

public class PrologueBlackBG : MonoBehaviour {

	// Use this for initialization
    public AudioClip accident;
    public Transform car;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void PlayBlackBG()
    {
        GetComponent<Animator>().Play("blacking");//播放背景开始变黑
    }
    void BlackEnd()
    {
        StartCoroutine(LoadLevel(5));
    }
    IEnumerator LoadLevel(float delayed)
    {
        yield return new WaitForSeconds(delayed-2.5f);
        AudioSource.PlayClipAtPoint(accident, car.position);
        yield return new WaitForSeconds(delayed);
        Application.LoadLevel("Accident");
    }
}
