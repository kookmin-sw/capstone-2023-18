using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private AICarAgent script;
    public float nowTime;
    

    public void Awake()
    {
        script = GetComponentInParent<AICarAgent>();
    }
    

    public void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Wall")){
            script.AddReward(-0.1f);  
            script.GoResetPos();
        }
        
        if(collider.gameObject.CompareTag("Goal")){
            script.AddReward(40f);
            script.EndEpisode();
        }

        if(collider.gameObject.CompareTag("Guide")){
            script.AddReward(-0.5f);
            script.GoResetPos();
        }

        if(collider.gameObject.CompareTag("Checkpoint")){
            
            if(script.cp == collider.gameObject){
                script.AddReward(-1f);
                script.EndEpisode();
            }
            else{
                
                Transform tmp = collider.gameObject.transform;
                tmp = tmp.GetChild(0).gameObject.transform;
                script.cp = collider.gameObject;
                script.SetResetPos(tmp);
                if(script.cpNum == script.cm.totalcp()){
                    script.cpNum = 0;
                }
                else{
                    script.cpNum++;
                }
                script.nextcp = script.cm.nextCheckpoint(script.cpNum);
                script.AddReward(5 + (script.cpNum * 0.5f));
            }
        }
    }
    public void OnTriggerStay(Collider collider){
        if(collider.gameObject.CompareTag("Way")){
            script.AddReward(2f/script.MaxStep);

        }
    }

    public void OnTriggerExit(Collider collider){
        if(collider.gameObject.CompareTag("Wall")){
            
        }
    }
    
   
    
    
}
