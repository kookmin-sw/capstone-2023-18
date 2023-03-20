using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;


public class AICarAgent : Agent
{
    
    private CheckpointManager cm;
    
    [Header("===Input===")]
    public float Hmove;
    public float Vmove;
    public bool Drift;
    public bool Item;
    private KartInput input;
    

    [Header("===Info===")] 
    [SerializeField] public int currentCheckpoint;
    [SerializeField] private GameObject nextCheckpoint;
    [SerializeField] private GameObject startPos;
    public int totalCheckpoint;
    private Transform agentTransform;
    private Rigidbody agentRigidbody;


    public override void Initialize()
    {
        currentCheckpoint = 0;
        
        
        cm = CheckpointManager.Instance;
        
        input = GetComponent<KartInput>();
        agentTransform = GetComponent<Transform>();
        agentRigidbody = GetComponent<Rigidbody>();
        
        
        Hmove = input.Hmove;
        Vmove = input.Vmove;

    }
    public override void OnEpisodeBegin()
    {
        //cp
        currentCheckpoint = 0;
        totalCheckpoint = cm.totalCheckPoint();
        
        //위치정보 초기화
        agentRigidbody.velocity = Vector3.zero;
        agentTransform.localRotation = Quaternion.Euler(0,00,0);
        agentTransform.position = startPos.transform.position;

    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentRigidbody.velocity); //3 차량 velocity
        sensor.AddObservation(agentTransform.position); //3 차량 위치
        sensor.AddObservation(transform.InverseTransformDirection(agentRigidbody.velocity).magnitude); //1 local velocity of car
        sensor.AddObservation(currentCheckpoint); //1 현재 체크포인트
        
        if (currentCheckpoint <= totalCheckpoint - 1) nextCheckpoint = cm.nextcheckPoint(currentCheckpoint); //다음 체크포인트
        sensor.AddObservation(nextCheckpoint.transform.position);
        
        Vector3 diff;
        diff = nextCheckpoint.transform.position - agentTransform.position;
        
        sensor.AddObservation(diff.magnitude); //1
        
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        input.Hmove = actionBuffers.ContinuousActions[0];
        input.Vmove = actionBuffers.ContinuousActions[1];
        input.Drift = actionBuffers.DiscreteActions[0] == 1 ? true : false;
        input.Item = actionBuffers.DiscreteActions[1] == 1 ? true : false;

        if(input.Vmove > 0) AddReward(input.Vmove * 0.01f);
        else AddReward(-0.1f);
        
        float handleAd = Mathf.Abs(input.Hmove * 0.001f);
        AddReward(-handleAd);
        AddReward(-0.01f);
    }
    
    public override void Heuristic(in ActionBuffers actionOut)
    {
        ActionSegment<float> continuousActions = actionOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionOut.DiscreteActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
        discreteActions[0] = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
        discreteActions[1] = Input.GetKey(KeyCode.Z) ? 1 : 0;
    }


    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-30f);
            //Debug.Log("wall");
            EndEpisode();
        }
    }
}
