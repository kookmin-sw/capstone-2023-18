using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private AICarAgent script;
    
    public void Start()
    {
        script = GetComponentInParent<AICarAgent>();
    }

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Checkpoint"))
        {
            if (other.gameObject.GetComponent<cp>().currentCnt == script.currentCheckpoint)
            {
                script.currentCheckpoint = other.gameObject.GetComponent<cp>().nextCnt;
                script.SetReward(1f * script.currentCheckpoint);
            }

            else
            {
                script.AddReward(-10f);
                script.EndEpisode();
            }
        }
        
        else if (other.CompareTag("Goal"))
        {
            if (script.currentCheckpoint < script.totalCheckpoint)
            {
                Debug.Log("wrong Way");
                script.AddReward(-30f);
            }
            else
            {
                Debug.Log("Goal");
                script.SetReward(script.totalCheckpoint * 1f);
            }
            script.EndEpisode();
        }
    }
}
