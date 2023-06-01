using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerInfo : NetworkBehaviour, IComparable<NetPlayerInfo>
{
    //�÷��̾��� ������ �˱� ���� ��ũ��Ʈ
    //Player Object Component�� ����Ǿ�, �� Player Object�� �ϳ��� ���� �ȴ�.
    //NetPlayerManager�� �� PlayerObject�� ã�� �ش� Component���� ������ ��, ���������� üũ�ϰ� �ȴ�.

    //�����ؾ� �� ����
    /*
     * LAP : �� ���� �������ΰ�
     * CP : ���� �ִ�� ����� CheckPoint
     * LAP TIME : �� ���� �� �ð� üũ
     * BEST TIME : �� ���� �� �ְ� ��� üũ
     * �ش� �������� Ŭ���̾�Ʈ�� ���� ���� �ְ�, �������� ��� �� �� �ֵ��� �Ѵ�. 
     */
    public NetworkVariable<int> myItem = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> myPosition = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<int> myCharacter = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> myRank = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> teamNumber = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<short> Lap = new NetworkVariable<short>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> LapTimes = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> BestTime = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> KMH = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<ulong> ID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> isFinish = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //��ŷ����Ʈ ���� �Ÿ�
    public NetworkVariable<float> CheckPointDistance = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> RpNum = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<Vector3> RpForward = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<Vector3> RpPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    //üũ����Ʈ
    public NetworkVariable<int> CpNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetKartInput input;
    public NetPlayManager npm; //Network Player Manager
    public NetPlayCheckPoint cpm; //Check Point Manager
    public bool isReturning;

    //���� ������
    public NetworkVariable<int> Item = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public bool touchedEndpoint;

    private void Awake()
    {
        input = gameObject.GetComponent<NetKartInput>();
        isReturning = false;
        StartCoroutine(FindComponent());
    }

    
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            CpNum.Value = 0;
            ID.Value = NetworkManager.Singleton.LocalClientId;
            NetPlayUI npi = GameObject.Find("@PlayManager").GetComponent<NetPlayUI>();
            npi.Player = gameObject.GetComponent<NetPlayerInfo>();
            npi.input = input;
            GameObject.Find("MiniMap_Camera").GetComponent<MinimapCamFollowing>().target = transform;
        }

        
    }

    IEnumerator FindComponent()
    {
        while (GameObject.Find("@PlayManager") == null)
        {
            yield return null;
        }
        GameObject.Find("@PlayManager").TryGetComponent(out npm);
        StartCoroutine(SetPointClients());
        StartCoroutine(SetNameTackClients());

        while (GameObject.Find("CheckPoints") == null)
        {
            yield return null;
        }
        GameObject.Find("CheckPoints").TryGetComponent(out cpm);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            CheckPointDistance.Value = Vector3.Dot(RpForward.Value, (transform.position - RpPosition.Value));
        }

        if(IsOwner && input.Return)
        {
            input.Return = false;
            if (isReturning == false)
            {
                StartCoroutine(ReturnToCP(transform, cpm.CP[CpNum.Value].transform));
            }
        }

        if (IsServer)
        {
            Item.Value = myItem.Value;
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

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("EndPoint") && IsOwner)
        {
            touchedEndpoint = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("CheckPoint_User") && IsOwner)
        {
            CP cpinfo = other.GetComponent<CP>();
            bool isCorrectRoute = false;
            //1. Chcek to Correct Next CheckPoint
            
            
            if(cpinfo == cpm.CP[CpNum.Value])
            {
                isCorrectRoute = true;
            }

            foreach(CP toNext in cpm.CP[CpNum.Value].NextCheckPoint)
            {
                if(toNext == cpinfo)
                {
                    isCorrectRoute = true;
                }
            }


            //if NO -> Warning and Don't Update to Checkpoint
            //if Yes -> Update to CheckPoint Keep going
            if(isCorrectRoute)
            {
                npm.UI.Warning.SetActive(false);
                CpNum.Value = cpinfo.CheckPointNum;
            }
            else
            {
                npm.UI.Warning.SetActive(true);
            }
        }

        if (other.CompareTag("EndPoint") && IsOwner && touchedEndpoint == false)
        {
            touchedEndpoint = true;
            if (CpNum.Value == (npm.MaxCP - 1))
            {
                //����
                Debug.Log("EndPoint" + gameObject.name);
                Lap.Value += 1;
                float LastTime = 0;
                float NowTime = npm.PlayTime.Value;
                LastTime = NowTime - LapTimes.Value;
                LapTimes.Value = NowTime;
                if (BestTime.Value == 0)
                {
                    BestTime.Value = LastTime;
                }
                else
                {
                    BestTime.Value = LastTime < BestTime.Value ? LastTime : BestTime.Value;
                }

                if (Lap.Value == npm.MaxLap)
                {
                    isFinish.Value = 1;
                    if (!npm.isClosing.Value)
                    {
                        npm.CloseGameServerRpc();
                    }
                }
            }

        }

    }


    IEnumerator SetPointClients()
    {
        NetPointerOnMap pointer = gameObject.GetComponentInChildren<NetPointerOnMap>();
        pointer.gameObject.SetActive(false);
        while (npm.isStart.Value == false)
        {
            yield return null;
        }
        pointer.gameObject.SetActive(true);
        if (IsOwner)
        {
            if (teamNumber.Value == 0)//Red Team
            {
                pointer.SetPointer(PointerType.MY_RED);
            }
            else//Blue Team
            {
                pointer.SetPointer(PointerType.MY_BLUE);
            }
        }
        else
        {
            if (teamNumber.Value == 0)//Red Team
            {
                pointer.SetPointer(PointerType.OTHER_RED);
            }
            else//Blue Team
            {
                pointer.SetPointer(PointerType.OTHER_BLUE);
            }
        }
    }

    IEnumerator SetNameTackClients()
    {
        NetShowNickName nick = gameObject.GetComponentInChildren<NetShowNickName>();
        while (npm.isStart.Value == false)
        {
            yield return null;
        }
        nick.SetNameTack("USER " + ID.Value.ToString(), teamNumber.Value);

    }

    public IEnumerator ReturnToCP(Transform _user, Transform _returnPoint)
    {
        if (IsOwner&&isReturning == false && npm.isStart.Value == true)
        {
            isReturning = true;
            Rigidbody rb = _user.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;

            // 최근 체크포인트로 카트의 위치/회전 변경
            _user.position = _returnPoint.position + new Vector3(0, 1, 0);
            _user.rotation = _returnPoint.rotation;

            //0.3초 이후 다시 움직이기.
            yield return new WaitForSecondsRealtime(0.3f);
            rb.constraints = RigidbodyConstraints.None;
            isReturning = false;
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