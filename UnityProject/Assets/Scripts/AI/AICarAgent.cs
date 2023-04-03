using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AICarAgent : Agent
{
    
    [Header("===Input===")]
    private KartInput input;
    public GameObject cp; // red line
    

    [Header("===Info===")] 
    [SerializeField] private Transform checkpoint; // front of red line


    private GameObject startPos;
    public Transform startLine;

    private Transform agentTransform;
    private Rigidbody agentRigidbody;


    //For check spinning
    private Vector3 nowPos; 
    private float driftTime;


    public override void Initialize()
    {
        startLine = GameObject.Find("startPos").transform;
        
        input = GetComponent<KartInput>();
        agentTransform = GetComponent<Transform>();
        agentRigidbody = GetComponent<Rigidbody>();
       
        checkpoint = startLine;

    }
    public override void OnEpisodeBegin()
    {
        driftTime = 0f;
        nowPos = agentTransform.position;
        cp = startPos;



        //위치정보 초기화
        agentRigidbody.velocity = Vector3.zero;
        agentRigidbody.angularVelocity = Vector3.zero;
        SetResetPos(startLine);
        GoResetPos();
        
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(checkpoint.position); //3 cp pos
        sensor.AddObservation(agentTransform.position); //3 차량 위치
        sensor.AddObservation(agentTransform.rotation); //3
        sensor.AddObservation(transform.InverseTransformDirection(agentRigidbody.velocity).magnitude); //1 local velocity of car
        
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        input.Vmove = 1f;
        input.Drift = actionBuffers.ContinuousActions[1] > 0 ? true : false;
        input.Hmove = actionBuffers.ContinuousActions[0];

        AddReward(3f/MaxStep);
    }
    
    public override void Heuristic(in ActionBuffers actionOut)
    {
        
        ActionSegment<float> continuousActions = actionOut.ContinuousActions;

        continuousActions[1]= Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;
        continuousActions[0] = Input.GetAxis("Horizontal");
 
        
           
    }
    

    public void FixedUpdate(){
        
        driftTime += Time.fixedDeltaTime;

        //check spin or don't move
        if(driftTime > 2f){
            float dis = Mathf.Abs((nowPos-agentTransform.localPosition).magnitude);
            
            if(dis < 10f){
                AddReward(-1f);
                GoResetPos();
            }
            driftTime = 0f;
            nowPos = agentTransform.localPosition;
        }
    }

    //set checkpoint
    public void SetResetPos(Transform tmp){
        checkpoint = tmp;
    }


    //move agent to cp
    public void GoResetPos(){
        agentRigidbody.velocity = Vector3.zero;
        agentRigidbody.angularVelocity = Vector3.zero;
        agentTransform.position = checkpoint.position + new Vector3(0f,0.3f,0f);
        agentTransform.rotation = checkpoint.rotation;
    }
}
