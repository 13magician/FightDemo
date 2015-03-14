using UnityEngine;
using System.Collections;

public class SpawnMonster : MonoBehaviour
{

    public GameObject[] arrMonster;
    public float spawnMinX = -17f, spawnMaxX = 5f;
    public float spawnY = 5f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (Random.Range(0, 50 * 3) == 0)
        {
            GameObject obj = Instantiate(arrMonster[Random.Range(0, arrMonster.Length)], new Vector3(Random.Range(spawnMinX, spawnMaxX), spawnY, 0), Quaternion.identity) as GameObject;
        }
    }
}
