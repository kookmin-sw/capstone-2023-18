using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRankPointNumber : MonoBehaviour
{
    CheckPoiintInfo[] RP;
    // Start is called before the first frame update
    void Start()
    {
        RP = gameObject.GetComponentsInChildren<CheckPoiintInfo>();

        for(int i=0; i<RP.Length; i++)
        {
            RP[i].CP_Num = i;
        }
    }
}
