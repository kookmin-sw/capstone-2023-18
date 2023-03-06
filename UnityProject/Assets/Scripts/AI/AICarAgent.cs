using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;


public class AICarAgent : Agent
{
    [Header("Input")]
    public float Hmove;
    public float Vmove;
    public bool Drift;
    public bool Item;
    private KartInput input;
    
    public override void Initialize()
    {
        input = GetComponent<KartInput>();
        Hmove = input.Hmove;
        Vmove = input.Vmove;

    }
    public override void OnEpisodeBegin()
    {
        
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        input.Hmove = actionBuffers.ContinuousActions[0];
        input.Vmove = actionBuffers.ContinuousActions[1];
        input.Drift = actionBuffers.DiscreteActions[0] == 1 ? true : false;
        Debug.Log(input.Drift);
    }
    
    public override void Heuristic(in ActionBuffers actionOut)
    {
        ActionSegment<float> continuousActions = actionOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionOut.DiscreteActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
        discreteActions[0] = Input.GetKeyUp(KeyCode.LeftControl) ? 1 : 0;

    }
}
