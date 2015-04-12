using UnityEngine;
using System.Collections;

public class AccidentAnimEnd : MonoBehaviour
{
    public AccidentPlayerCreep creep;
    public Transform player;
    public AudioClip heartbeat,pain;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void AnimEnd()
    {
        creep.canWalk = true;
        creep.isShow = true;
        AudioSource.PlayClipAtPoint(pain, player.position);
    }
    void PlayHeartbeat()
    {
        AudioSource.PlayClipAtPoint(heartbeat, player.position);
    }
}
