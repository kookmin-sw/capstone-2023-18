using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private AICarAgent script;
    public float nowTime;
    
    public void Start()
    {
        script = GetComponentInParent<AICarAgent>();
    }

    private void FixedUpdate()
    {
        nowTime += Time.fixedDeltaTime;

        
        if (nowTime > 3f)
        {
            nowTime = 0f;
            script.AddReward(-60f);
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
                script.AddReward(+20f);
            }

            else
            {
                script.AddReward(-100f);
                //script.AddReward(-1f * nowTime * 0.1f);
                script.EndEpisode();
            }
            
            
            nowTime = 0f;
        }
        else if (other.CompareTag("Guide"))
        {
            script.AddReward(-200f);
            script.EndEpisode();
        }
        else if (other.CompareTag("Goal"))
        {
            if (script.currentCheckpoint < script.totalCheckpoint-1)
            {
                script.AddReward(-200f);
            }
            else
            {
                script.SetReward(script.totalCheckpoint * 5f);
            }
            script.EndEpisode();
        }
    }
    
}
