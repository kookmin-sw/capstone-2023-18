using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private AICarAgent script;
    private float nowTime;
    
    public void Start()
    {
        script = GetComponentInParent<AICarAgent>();
    }

    private void FixedUpdate()
    {
        nowTime += Time.fixedDeltaTime;

        if (nowTime > 10f)
        {
            nowTime = 0f;
            script.AddReward(-20f);
            script.EndEpisode();
        }
        
    }

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Checkpoint"))
        {
            
            if (other.gameObject.GetComponent<cp>().currentCnt == script.currentCheckpoint)
            {
                script.currentCheckpoint = other.gameObject.GetComponent<cp>().nextCnt;
                script.AddReward(2f * (script.currentCheckpoint + 1));
            }

            else
            {
                script.AddReward(-50f);
                script.EndEpisode();
            }
            
            script.AddReward(-1f * nowTime * 0.1f);
            Debug.Log(nowTime);
            nowTime = 0f;
        }
        
        else if (other.CompareTag("Goal"))
        {
            if (script.currentCheckpoint < script.totalCheckpoint-1)
            {
                Debug.Log("wrong Way");
                script.AddReward(-30f);
            }
            else
            {
                Debug.Log("Goal");
                script.SetReward(script.totalCheckpoint * 5f);
            }
            script.EndEpisode();
        }
    }
}
