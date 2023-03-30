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
    
    public void OnTriggerStay (Collider collider)
    {
        if (collider.gameObject.CompareTag("Wall"))
        {
            script.AddReward(-1f);
            script.EndEpisode();

        }
    }

    public void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Goal")){
            script.AddReward(1f);
            script.EndEpisode();
        }

        if(collider.gameObject.CompareTag("Guide")){
            script.AddReward(-2f);
            script.EndEpisode();
        }

        if(collider.gameObject.CompareTag("Checkpoint")){
            Transform tmp = collider.gameObject.transform;
            script.SetResetPos(tmp);
        }
    }
    
    
    
    
}
