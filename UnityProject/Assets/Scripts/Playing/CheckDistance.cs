using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckDistance : MonoBehaviour
{
    public int MaxLap; //맵당 최대 바퀴
    public int NowLap; //현재 랩수

    public float CheckPointDistance; // 체크포인트 기준 거리

    public Vector3 LastCP;
    public Vector3 LastCPPos;
    public int LastCPNumber;

    public TextMeshProUGUI disText;

    void Start()
    {
        MaxLap = 3;
        NowLap = 0;

        LastCP = GameObject.Find("CP00").gameObject.GetComponent<CheckPoiintInfo>().forward;
        LastCPPos = GameObject.Find("CP00").transform.position;
        LastCPNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPointDistance = Vector3.Dot(LastCP, (transform.position - LastCPPos));
        disText.text = gameObject.name + " " + NowLap + " " + LastCPNumber + " " + CheckPointDistance;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Checkpoint"))
        {
            CheckPoiintInfo cpinfo = other.GetComponent<CheckPoiintInfo>();

            if(LastCPNumber != cpinfo.CP_Num)
            {
                LastCP = cpinfo.forward;
                LastCPNumber = cpinfo.CP_Num;
                LastCPPos = cpinfo.centerPos;
            }
        }
    }
}
