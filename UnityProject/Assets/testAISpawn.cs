using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class testAISpawn : NetworkBehaviour
{
    public GameObject ai;

    public GameObject startPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnTimer(2f));
    }

    IEnumerator spawnTimer(float time)
    {
        while (time > 0)
        {
            Debug.Log(time);
            time -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        GameObject tmp = (GameObject)Instantiate(ai, startPos.transform.position + new Vector3(0f,0.3f,0f), startPos.transform.rotation);
        
    }
}
