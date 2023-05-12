using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System.Linq;
using PowerslideKartPhysics;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class NetPlayManager : NetworkBehaviour
{
    public GameObject[] KartPrefab;
    public LobbyOrchestrator LO;
    //���� ���� ��ġ
    public GameObject[] StartingPoints;

    // ��ü������ �˾ƾ� �ϴ°�
    public NetworkVariable<bool> isStart; //���� ������ �����ߴ���
    public NetworkVariable<float> PlayTime; // ���� ���� �ð�.
    public int UserCount = 0;

    //�÷��� ���൵ üũ
    //�� �÷��̾� ���� üũ �� �� �ֵ��� �Ѵ�.
    public int MaxLap; // �� ���� ������
    public int MaxCP; // �ش� �ʿ� CP�� �� �� �ִ���.

    //�÷��̾ ���൵
    public SortedDictionary<ulong, NetPlayerInfo> Players = new SortedDictionary<ulong, NetPlayerInfo>();

    //����
    //���� ���� ��ġ
    public static NetPlayManager instance;

    //����
    public NetworkList<ulong> rank;

    public NetPlayUI UI;
    private void Awake()
    {
        rank = new NetworkList<ulong>();
        UI = gameObject.GetComponent<NetPlayUI>();
    }
    public override void OnNetworkSpawn()
    {
        setMapInfo();
        LO = LobbyOrchestrator.Instance.GetComponent<LobbyOrchestrator>();
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    void Init()
    {
        if (instance == null)
        {
            GameObject gm = GameObject.Find("@PlayManager");
            if (gm == null)
            {
                gm = new GameObject { name = "@PlayManager" };
            }

            if (gm.GetComponent<NetPlayManager>() == null)
            {
                gm.AddComponent<NetPlayManager>();
            }
            DontDestroyOnLoad(gm);
            instance = gm.GetComponent<NetPlayManager>();
        }
    }
    private void Update()
    {
        if (IsServer)
        {
            //�׽�Ʈ �ڵ�
            if (Input.GetKeyDown(KeyCode.F5) && isStart.Value == false)
            {
                StartGameButton();
            }

            if (isStart.Value)
            {
                PlayTime.Value += Time.deltaTime;
                GetRank();
            }
        }

    }


    //�׽�Ʈ �ڵ� �ӽ÷� �����ϱ� ����.
    void StartGameButton()
    {
        StartCoroutine(StartCountDown());
        ItemManager.instance.GetAllKarts();
    }

    public IEnumerator StartCountDown()
    {
        ItemManager.instance.GetAllKarts();
        for (int i = 3; i > 0; i--)
        {
            //ui.Count.text = i.ToString();
            UI.CountdownClientRPC(i);
            yield return new WaitForSeconds(1);
        }
        UI.CountdownClientRPC(0);
        isStart.Value = true;
    }

    void setMapInfo()
    {
        //������ ���� ������ �޾ƿͼ� ����Ѵ�.
        MaxLap = 1;
        StartingPoints = GameObject.FindGameObjectsWithTag("StartPoint");
    }

    //��ŷ ���
    void GetRank()
    {
        //1. LAP �켱 ����
        //2. CHECK POINT �� �켱 ����
        //3. CHECK POINT ���� �� �Ÿ� ��� ���

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
            Players[key].myRank.Value = i;
            i++;
        }

    }

    //�� �÷��̾� ���� Ȯ��.

    //Player ����� ������ ���� �ִ� Player �����Ϳ� �߰�.
    [ServerRpc(RequireOwnership = false)]
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
            AddPlayerClientRpc(uid, StartingPoints[UserCount].transform.position);
            UserCount += 1;
        }
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    public void AddPlayerClientRpc(ulong _uid, Vector3 _pos, ClientRpcParams rpcParams = default)
    {
        if (NetworkManager.Singleton.LocalClientId == _uid)
        {
            Debug.Log($"Player {NetworkManager.Singleton.LocalClientId} ��ġ����");
            NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(_uid).GetComponent<Transform>().position = _pos;
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void CloseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        OnGameClose();
    }
    //���� ����

    private async void OnGameClose()
    {
        using (new Load("Closing the game..."))
        {
            await MatchmakingService.UnLockLobby();
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId)
    {
        var spawn = Instantiate(KartPrefab[LO._playersInLobby[playerId].KartIndex]); // ������ īƮ
        spawn.transform.position = StartingPoints[UserCount].transform.position;
        spawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(playerId);
        NetPlayerInfo userinfo = spawn.GetComponent<NetPlayerInfo>();
        Players.Add(playerId, userinfo);
        rank.Add(playerId);
        UserCount += 1;
        Debug.Log(LO._playersInLobby[playerId]);
        //TODO
        userinfo.myCharacter.Value = LO._playersInLobby[playerId].CharacterIndex;
        if (LO._playersInLobby[playerId].isRedTeam) userinfo.teamNumber.Value = 0;
        else userinfo.teamNumber.Value = 1;
        userinfo.myPosition.Value = (int)LO._playersInLobby[playerId].position;



    }
}