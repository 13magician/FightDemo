using UnityEngine;
using System.Collections;

public class AccidentNextSceneTrigger : MonoBehaviour {

	// Use this for initialization
    public Animator changeBlackAnim;
    public AudioClip frighten;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D hit)
    {
        Debug.Log("enter");
        if(hit.transform.root.tag=="Player")
        {
            changeBlackAnim.Play("ChangeBlack");
            StartCoroutine(LoadNextScene());
        }
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3.5f);
        AudioSource.PlayClipAtPoint(frighten, transform.position);
        yield return new WaitForSeconds(2.5f);
        Application.LoadLevel("s1");//加载下一关
    }
}
