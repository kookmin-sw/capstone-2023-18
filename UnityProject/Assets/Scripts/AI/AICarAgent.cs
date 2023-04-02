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
    public float Hmove;
    public float Vmove;
    public bool Drift;
    public bool Item;
    private KartInput input;
    

    [Header("===Info===")] 
    
    [SerializeField] private Transform checkpoint;
    [SerializeField] private GameObject startPos;

    private Transform startLine;
    private Transform agentTransform;
    private Rigidbody agentRigidbody;
    private float driftTime;

    private Vector3 nowPos; // For check spin



    public override void Initialize()
    {
        startLine = GameObject.Find("startLine").transform;
        
        input = GetComponent<KartInput>();
        agentTransform = GetComponent<Transform>();
        agentRigidbody = GetComponent<Rigidbody>();
       
        checkpoint = startLine;

        Hmove = input.Hmove;
        Vmove = input.Vmove;

    }
    public override void OnEpisodeBegin()
    {

        driftTime = 0f;
        nowPos = agentTransform.localPosition;
        //위치정보 초기화
        agentRigidbody.velocity = Vector3.zero;
        agentTransform.localRotation = Quaternion.Euler(0,0,0);
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
        input.Hmove = actionBuffers.ContinuousActions[0];
        input.Drift = actionBuffers.DiscreteActions[0] == 1 ? true : false;
        //input.Item = actionBuffers.DiscreteActions[1] == 1 ? true : false;
        
        AddReward(2f/MaxStep);
    }
    
    public override void Heuristic(in ActionBuffers actionOut)
    {
        ActionSegment<float> continuousActions = actionOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionOut.DiscreteActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        discreteActions[0] = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
        //discreteActions[1] = Input.GetKey(KeyCode.Z) ? 1 : 0;
    }
    
    public void FixedUpdate(){
        
        driftTime += Time.fixedDeltaTime;

        if(driftTime > 1f){
            //Debug.Log(Mathf.Abs((nowPos-agentTransform.localPosition).magnitude));
            if(Mathf.Abs((nowPos-agentTransform.localPosition).magnitude) < 10f){
                //Debug.Log("spin");
                SetReward(-2f);
                EndEpisode();
            }
            driftTime = 0f;
            nowPos = agentTransform.localPosition;
        }
    }

    public void SetResetPos(Transform tmp){
        checkpoint = tmp;
    }

    private void GoResetPos(){
        
        Debug.Log(checkpoint.rotation);
        agentTransform.position = checkpoint.position + new Vector3(0.5f,1.5f,0.5f);
       


    }
}
