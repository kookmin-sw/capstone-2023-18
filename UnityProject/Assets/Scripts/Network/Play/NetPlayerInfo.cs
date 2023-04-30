using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerInfo : NetworkBehaviour, IComparable<NetPlayerInfo>
{
    //플레이어의 정보를 알기 위한 스크립트
    //Player Object Component로 적용되어, 각 Player Object는 하나씩 갖게 된다.
    //NetPlayerManager는 각 PlayerObject를 찾고 해당 Component값을 저장한 후, 지속적으로 체크하게 된다.

    //관리해야 할 정보
    /*
     * LAP : 몇 바퀴 진행중인가
     * CP : 현재 최대로 진행된 CheckPoint
     * LAP TIME : 랩 구간 당 시간 체크
     * BEST TIME : 랩 구간 당 최고 기록 체크
     * 해당 변수들은 클라이언트는 읽을 수만 있고, 서버만이 기록 할 수 있도록 한다. 
     */

    public NetworkVariable<short> Lap = new NetworkVariable<short>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkList<float> LapTimes;
    public NetworkVariable<float> BestTime = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> KMH = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //랭킹포인트 기준 거리
    public NetworkVariable<float> CheckPointDistance = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> RpNum = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<Vector3> RpForward = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<Vector3> RpPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    //체크포인트
    public NetworkVariable<int> CpNum = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetPlayManager npm;

    //보유 아이템
    public NetworkVariable<int> Item = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    private void Awake()
    {
        LapTimes = new NetworkList<float>();
    }

    void Start()
    {
        npm = GameObject.Find("@PlayManager").GetComponent<NetPlayManager>();
        if(IsOwner)
        {
            GameObject.Find("@PlayManager").GetComponent<NetPlayUI>().Player = gameObject.GetComponent<NetPlayerInfo>();
            npm.AddPlayerServerRpc();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsServer)
        {
            CheckPointDistance.Value = Vector3.Dot(RpForward.Value, (transform.position - RpPosition.Value));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Rankpoint") && IsServer)
        {
            CheckPoiintInfo cpinfo = other.GetComponent<CheckPoiintInfo>();

            if (RpNum.Value != cpinfo.CP_Num)
            {
                RpForward.Value = cpinfo.forward;
                RpNum.Value = cpinfo.CP_Num;
                RpPosition.Value = cpinfo.centerPos;
                
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Checkpoint"))
        {
            CP cpinfo = other.GetComponent<CP>();
            CpNum.Value = cpinfo.CheckPointNum;
        }

        if (other.CompareTag("EndPoint") && IsServer)
        {
            if (CpNum.Value == (npm.MaxCP - 1))
            {
                //도착
                Lap.Value += 1;
                int lc = LapTimes.Count;
                float LastTime = 0;
                LastTime = npm.PlayTime.Value - (lc > 0 ? LapTimes[lc - 1] : 0);
                LapTimes.Add(LastTime);

                if (BestTime.Value == 0)
                {
                    BestTime.Value = LastTime;
                }
                else
                {
                    BestTime.Value = LastTime < BestTime.Value ? LastTime : BestTime.Value;
                }

                if(Lap.Value == npm.MaxLap)
                {
                    npm.CloseGameServerRpc();
                }
            }

        }
    }

    public int CompareTo(NetPlayerInfo other)
    {
        if (other == null) return 1;

        int lapComparison = Lap.Value.CompareTo(other.Lap.Value);
        if (lapComparison != 0) return -lapComparison;

        int rpNumComparison = RpNum.Value.CompareTo(other.RpNum.Value);
        if (rpNumComparison != 0) return -rpNumComparison;

        return CheckPointDistance.Value.CompareTo(other.CheckPointDistance.Value);
    }
}
