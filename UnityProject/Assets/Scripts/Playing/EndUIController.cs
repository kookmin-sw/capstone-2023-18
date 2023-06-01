using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EndUIController : NetworkBehaviour
{
    [Space, Header("Child Objects")]
    public GameObject UI;
    public Transform Camera;

    [Space, Header("End_UI")]
    public Vector3 CameraOffset = new Vector3(0, 0.5f, 2.5f);
    public TextMeshProUGUI Banner;
    public TextMeshProUGUI RedScore;
    public TextMeshProUGUI BlueScore;
    public Sprite[] SourceImages; //00 ->Btn_red 01 ->Btn_blue
    public GameObject[] EndRanks;
    public TextMeshProUGUI[] EndRanks_ID;
    public TextMeshProUGUI[] EndRanks_Time;
    public Image[] EndRankds_BG;

    NetPlayManager npm;

    int[] CountScore = { 10, 8, 6, 5, 4, 3, 2, 1 };

    public void Start()
    {
        npm = GameObject.Find("@PlayManager").gameObject.GetComponent<NetPlayManager>();
    }
    
    public void OnScoreBoard()
    {
        UI.SetActive(true);
        Camera.gameObject.SetActive(true);
        for (int i = 0; i < EndRanks.Length; i++)
        {
            EndRanks[i].SetActive(false);
        }
        if (IsServer)
        {
            UpdateRankServer();
        }
    }

    public void UpdateRankServer()
    {

        Transform mvpTransform = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(npm.rank[0]).GetComponent<Transform>();
        Vector3 MvpPosition = mvpTransform.position;
        Quaternion MvpRotation = mvpTransform.rotation;
        int Rank;
        float LapTime;
        int Team;
        ulong UID;
        int RedScore = 0, BlueScore = 0;
        int RedFirstRank = -1, BlueFirstRank = -1;

        for(int i=0; i<npm.rank.Count; i++)
        {
            NetPlayerInfo user = npm.Players[npm.rank[i]];
            UID = npm.rank[i];
            Rank = i;
            LapTime = user.isFinish.Value == 1 ? user.LapTimes.Value : 0;
            Team = user.teamNumber.Value;

            if(Team == 0)
            {
                //RedTeam
                RedScore +=  CountScore[i];
                if (RedFirstRank == -1) RedFirstRank = i;
            }
            else
            {
                //BlueTeam
                BlueScore += CountScore[i];
                if (BlueFirstRank == -1) BlueFirstRank = i;
            }
            UpdateRankClientRpc(Rank, LapTime, Team, UID);
        }

        string _banner;
        if(RedScore > BlueScore)
        {
            _banner = "RED TEAM WIN";
        }
        else if(RedScore < BlueScore)
        {
            _banner = "BLUE TEAM WIN";
        }
        else
        {
            _banner = RedFirstRank < BlueFirstRank ? "RED TEAM WIN" : "BLUE TEAM WIN";
        }

        UpdateWinTeamClientRpc(_banner, RedScore, BlueScore, MvpPosition, MvpRotation);
        Invoke("NpmCLoseGame", 10);
    }

    void NpmCLoseGame()
    {
        npm.OnGameClose();
    }
    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    public void LeaveGameClientRpc()
    {
        npm.OnGameClose();
    }


    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    public void UpdateRankClientRpc(int _rank, float _Laptime, int _team, ulong _uid)
    {
        EndRanks[_rank].SetActive(true);
        EndRanks_ID[_rank].text = "USER " + _uid;
        EndRanks_Time[_rank].text = _Laptime == 0 ? "RETIRE" : string.Format("{0:0}:{1:00}.{2:000}",
                     Mathf.Floor(_Laptime / 60),//minutes
                     Mathf.Floor(_Laptime) % 60,//seconds
                     Mathf.Floor((_Laptime * 1000) % 1000));//miliseconds
        EndRankds_BG[_rank].sprite = SourceImages[_team];
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    public void UpdateWinTeamClientRpc(string _banner, int _redScore, int _blueScore, Vector3 _mvpPosition, Quaternion _mvpRotation)
    {
        Camera.position = _mvpPosition + CameraOffset;
        Camera.LookAt(_mvpPosition);
        

        Banner.text = _banner;
        Banner.color = _banner == "RED TEAM WIN" ? Color.red : Color.blue;
        RedScore.text = _redScore.ToString();
        BlueScore.text = _blueScore.ToString();
    }


}
