using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class NetPlayManager : NetworkBehaviour
{
    //시작 세팅 위치
    public GameObject[] StartingPoints;

    // 전체적으로 알아야 하는것
    public NetworkVariable<bool> isStart; //현재 게임이 시작했는지
    public NetworkVariable<float> PlayTime; // 현재 주행 시간.
    public int UserCount = 0;

    //플레이 진행도 체크
    //각 플레이어 별로 체크 할 수 있도록 한다.
    public int MaxLap; // 몇 바퀴 맵인지
    public int MaxCP; // 해당 맵에 CP가 몇 개 있는지.

    //플레이어별 진행도
    private SortedDictionary<ulong, NetPlayerInfo> Players = new SortedDictionary<ulong, NetPlayerInfo>();

    //순위
    public NetworkList<ulong> rank;


    public void Awake()
    {
        rank = new NetworkList<ulong>();
    }

    private void Start()
    {

        setMapInfo();
    }

    private void Update()
    {
        if(IsServer)
        {
            //테스트 코드
            if(Input.GetKeyDown(KeyCode.F5) && isStart.Value == false)
            {
                StartGameButton();
            }

            if(isStart.Value)
            {
                PlayTime.Value += Time.deltaTime;
                GetRank();
            }
        }

    }

    //테스트 코드 임시로 시작하기 위함.
    void StartGameButton()
    {
        StartCoroutine(StartCountDown());
    }

    public IEnumerator StartCountDown()
    {
        NetPlayUI ui = GetComponent<NetPlayUI>();
        for(int i=3; i>0; i--)
        {
            ui.Count.text = i.ToString();
            ui.CountdownClientRPC(i);
            yield return new WaitForSeconds(1);
        }
        ui.CountdownClientRPC(0);
        isStart.Value = true;
    }

    void setMapInfo()
    {
        //맵으로 부터 정보를 받아와서 등록한다.
        MaxLap = 1;
        MaxCP = GameObject.FindGameObjectsWithTag("Checkpoint").Count();
        StartingPoints = GameObject.FindGameObjectsWithTag("StartPoint");
    }

    //랭킹 계산
    void GetRank()
    {
        //1. LAP 우선 정렬
        //2. CHECK POINT 수 우선 정렬
        //3. CHECK POINT 동일 시 거리 비례 계산

        /*
        var _rank = from pair in Players
                       orderby pair.Value.Lap, pair.Value.RpNum, pair.Value.CheckPointDistance
                       select pair;

        */
        var _rank = Players.OrderByDescending(r => r.Value, new NetPlayerInfoComparer())
                   .Select(r => r.Key)
                   .ToArray();

        int i = 0;
        foreach (ulong key in _rank)
        {
            rank[i] = _rank[i];
            i++;
        }

    }


    //각 플레이어 주행 확인.

    //Player 입장시 서버가 갖고 있는 Player 데이터에 추가.
    [ServerRpc (RequireOwnership = false)]
    public void AddPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong uid = serverRpcParams.Receive.SenderClientId;

        if (!Players.ContainsKey(uid))
        {
            NetworkObject user = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid);
            NetPlayerInfo userinfo = user.GetComponent<NetPlayerInfo>();

            Players.Add(uid, userinfo);
            rank.Add(uid);
            Debug.Log(user.name);
            user.transform.position = StartingPoints[UserCount].transform.position;
            UserCount += 1;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CloseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        OnGameClose();
    }
    //게임 종료

    private async void OnGameClose()
    {
        using (new Load("Closing the game..."))
        {
            await MatchmakingService.UnLockLobby();
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }
}
