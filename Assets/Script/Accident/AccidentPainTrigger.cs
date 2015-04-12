using UnityEngine;
using System.Collections;

public class AccidentPainTrigger : MonoBehaviour {
    public AudioClip pain2;
    float PainInterval = 2.5f;
    float Interval = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Interval += Time.deltaTime;
	}
    void OnTriggerEnter2D(Collider2D hit)
    {
        if(hit.transform.root.tag=="Player")
        {
            if(Interval>PainInterval)
            {
                AudioSource.PlayClipAtPoint(pain2, transform.position);
                Interval = 0f;
            }
        }
    }
}
