using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartItemMovement : MonoBehaviour
{
    public bool isSpin = false;
    Vector3 spinForward = Vector3.forward;
    Vector3 spinUp = Vector3.up;
    Vector3 spinOffset = Vector3.zero;
    public float spinRate = 15f;
    public float spinHeight = 1.0f;
    //rotation dir
    public enum SpinAxis { Yaw, Pitch, Roll}

    
}
