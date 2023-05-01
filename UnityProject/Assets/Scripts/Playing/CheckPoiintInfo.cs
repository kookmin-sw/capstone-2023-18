using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoiintInfo : MonoBehaviour
{
    public int CP_Num;

    public Vector3 forward;
    public Vector3 centerPos;

    // Start is called before the first frame update
    void Start()
    {
        forward = transform.forward ;
        centerPos = transform.position;
    }

}
