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
    public GameObject nextcp;
    private Transform checkpoint; // front of red line
    public int cpNum;

    public AIcheckpoint cm;

    [Header("===Info===")] 

    private bool isSpin;
    public bool onWall;
    private bool isWrongway;
    private float distanceFromcp;
    
    private GameObject startPos;
    public Transform startLine;

    private Transform agentTransform;
    private Rigidbody agentRigidbody;


    //For check spinning
    private Vector3 nowPos; 
    private float driftTime;
    private float dir; // 내적

    public override void Initialize()
    {
        
        cpNum = 0;
        
        cm = GameObject.Find("aicp").GetComponent<AIcheckpoint>();

        startLine = GameObject.Find("startPos").transform;

        input = GetComponent<KartInput>();
        agentTransform = GetComponent<Transform>();
        agentRigidbody = GetComponent<Rigidbody>();
        
        checkpoint = startLine;
        

    }
    public override void OnEpisodeBegin()
    {
        checkpoint = startLine;
        cp = startPos;
        cpNum = 0;
        nextcp = cm.nextCheckpoint(cpNum);
        reset();
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
        sensor.AddObservation(isWrongway); //1
        sensor.AddObservation(isSpin); //1
        sensor.AddObservation(cpNum);//1
        sensor.AddObservation(nextcp.transform.position);//3
        sensor.AddObservation((agentTransform.position - nextcp.transform.position).magnitude);//1
        sensor.AddObservation(agentTransform.forward); // 3
        sensor.AddObservation(dir); //1
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        input.Vmove = 1f;
        input.Drift = actionBuffers.ContinuousActions[1] > 0 ? true : false;
        input.Hmove = actionBuffers.ContinuousActions[0];

        AddReward(-1f/MaxStep);
    }
    
    public override void Heuristic(in ActionBuffers actionOut)
    {
        
        ActionSegment<float> continuousActions = actionOut.ContinuousActions;

        continuousActions[1]= Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;
        continuousActions[0] = Input.GetAxis("Horizontal");
       
           
    }
    
    public void reset(){
        driftTime = 0f;
        isSpin = false;
        onWall = false;
        isWrongway = false;
        agentRigidbody.velocity = Vector3.zero;
        agentRigidbody.angularVelocity = Vector3.zero;
        input.Hmove = 0f;
        input.Vmove = 0f;
        input.Drift = false;
        nowPos =  checkpoint.position + new Vector3(0f,0.3f,0f);
    }

    public void FixedUpdate(){
        //Debug.Log(nextcp.transform.localPosition);
        dir = Vector3.Dot(agentTransform.forward,checkpoint.forward);
        if(dir < -0.8f){
            isWrongway = true;
            AddReward(-2f);
            GoResetPos();
            
        }
        else{
            isWrongway = false;
        }

        driftTime += Time.fixedDeltaTime;

        //check spin or don't move
        if(driftTime > 1f){
            float dis = Mathf.Abs((nowPos-agentTransform.localPosition).magnitude);
            if(dis < 15f){
                isSpin = true;
                AddReward(-5f);
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
        reset();
       
        agentRigidbody.velocity = Vector3.zero;
        agentRigidbody.angularVelocity = Vector3.zero;
        agentTransform.position = checkpoint.position + new Vector3(0f,0.3f,0f);
        agentTransform.rotation = checkpoint.rotation;
    }
}
