using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS4014

/// <summary>
///     Lobby orchestrator. I put as much UI logic within the three sub screens,
///     but the transport and RPC logic remains here. It's possible we could pull
/// </summary>
public class LobbyOrchestrator : NetworkBehaviour {
    [SerializeField] public MainLobbyScreen _mainLobbyScreen;
    [SerializeField] public CreateLobbyScreen _createScreen;
    [SerializeField] public RoomScreen _roomScreen;
    [SerializeField] public int _countTeam; // Team 카운트 0이하 -> Red팀, 0초과 -> Blue팀 배정, \
    [SerializeField] public String _mapName;

    public static GameObject Instance { get; set; }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Debug.Log("New Lobby");
            Instance = gameObject;
            _mainLobbyScreen.gameObject.SetActive(true);
            _createScreen.gameObject.SetActive(false);
            _roomScreen.gameObject.SetActive(false);

            CreateLobbyScreen.LobbyCreated -= CreateLobby;
            CreateLobbyScreen.LobbyCreated += CreateLobby;

            LobbyRoomPanel.LobbySelected -= OnLobbySelected;
            LobbyRoomPanel.LobbySelected += OnLobbySelected;

            RoomScreen.LobbyLeft -= OnLobbyLeft;
            RoomScreen.LobbyLeft += OnLobbyLeft;

            RoomScreen.StartPressed -= OnGameStart;
            RoomScreen.StartPressed += OnGameStart;

        }
        else
        {
            Debug.Log("Destroy Lobby");
            Destroy(gameObject);
        }

        if (MatchmakingService.isJoiningLobby())
        {
            var lo = Instance.GetComponent<LobbyOrchestrator>();
            lo._mainLobbyScreen.gameObject.SetActive(false);
            lo._createScreen.gameObject.SetActive(false);
            lo._roomScreen.gameObject.SetActive(true);
        }
        else
        {
            _countTeam = 0;
            var lo = Instance.GetComponent<LobbyOrchestrator>();
            lo._mainLobbyScreen.gameObject.SetActive(true);
            lo._createScreen.gameObject.SetActive(false);
            lo._roomScreen.gameObject.SetActive(false);
        }
        GameManager.Sound.BGMPlay(GameManager.Sound.BGMList[(int)BGMLIST.LOBBY]);
    }

    #region Main Lobby

    private async void OnLobbySelected(Lobby lobby) {
        using (new Load("Joining Lobby...")) {
            try {
                await MatchmakingService.JoinLobbyWithAllocation(lobby.Id);

                _mainLobbyScreen.gameObject.SetActive(false);
                _roomScreen.gameObject.SetActive(true);
                NetworkManager.Singleton.StartClient();
            }
            catch (Exception e) {
                Debug.LogError(e);
                CanvasUtilities.Instance.ShowError("Failed joining lobby");
            }
        }
    }

 

    #endregion

    #region Create

    private async void CreateLobby(LobbyData data) {
        using (new Load("Creating Lobby...")) {
            try {
                await MatchmakingService.CreateLobbyWithAllocation(data);
                _mapName = data.Map;
                _createScreen.gameObject.SetActive(false);
                _roomScreen.gameObject.SetActive(true);
                // Starting the host immediately will keep the relay server alive
                NetworkManager.Singleton.StartHost();
            }
            catch (Exception e) {
                Debug.LogError(e);
                CanvasUtilities.Instance.ShowError("Failed creating lobby");
            }
        }
    }

    #endregion

    #region Room


    public readonly Dictionary<ulong, LobbyPlayerInfo> _playersInLobby = new();
    public static event Action<Dictionary<ulong, LobbyPlayerInfo>> LobbyPlayersUpdated;
    private float _nextLobbyUpdate;

    public override void OnNetworkSpawn() {
        if (IsServer) {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            LobbyPlayerInfo newPlayer;
            if (_countTeam <= 0)
            {
                Debug.Log("Server Created Room");
                newPlayer = new LobbyPlayerInfo();
                if(NetworkManager.Singleton.IsHost)
                {
                    newPlayer.isHost = true;
                }
                else
                {
                    newPlayer.isHost = false;
                }
                newPlayer.isRedTeam = true;
                _countTeam += 1;
            }
            else
            {
                newPlayer = new LobbyPlayerInfo();
                if (NetworkManager.Singleton.IsHost)
                {
                    newPlayer.isHost = true;
                }
                else
                {
                    newPlayer.isHost = false;
                }
                newPlayer.isRedTeam = false;
                _countTeam -= 1;
            }
            var uid = NetworkManager.Singleton.LocalClientId;
            if (_playersInLobby.ContainsKey(uid))
            {
                _playersInLobby[uid] = newPlayer;
            }
            else
            {
                _playersInLobby.Add(uid, newPlayer);

            }
            UpdateInterface();
        }

        // Client uses this in case host destroys the lobby
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

 
    }

    private void OnClientConnectedCallback(ulong playerId) {
        if (!IsServer) return;

        // Add locally
        if (!_playersInLobby.ContainsKey(playerId))
        {
            LobbyPlayerInfo newPlayer;
            if (_countTeam <= 0)
            {
                Debug.Log("chec1");
                newPlayer = new LobbyPlayerInfo();
                newPlayer.isRedTeam = true;
                _countTeam += 1;
            }
            else
            {
                Debug.Log("chec2");
                newPlayer = new LobbyPlayerInfo();
                newPlayer.isRedTeam = false;
                _countTeam -= 1;
            }
            _playersInLobby.Add(playerId, newPlayer);
        }


        PropagateToClients();

        UpdateInterface();
    }

    public void PropagateToClients() {
        foreach (var player in _playersInLobby) UpdatePlayerClientRpc(player.Key, player.Value);
    }

    [ClientRpc]
    private void UpdatePlayerClientRpc(ulong clientId, LobbyPlayerInfo userInfo) {
        if (IsServer) return;
        Debug.Log($"{clientId} is color" + (userInfo.isRedTeam ? "RED" : "BLUE"));
        if (!_playersInLobby.ContainsKey(clientId)) _playersInLobby.Add(clientId, userInfo);
        else _playersInLobby[clientId] = userInfo;
        UpdateInterface();
    }

    private void OnClientDisconnectCallback(ulong playerId) {
        if (IsServer) {
            // Handle locally
            if (_playersInLobby.ContainsKey(playerId)) _playersInLobby.Remove(playerId);

            // Propagate all clients
            RemovePlayerClientRpc(playerId);

            UpdateInterface();
        }
        else {
            // This happens when the host disconnects the lobby
            _roomScreen.gameObject.SetActive(false);
            _mainLobbyScreen.gameObject.SetActive(true);
            OnLobbyLeft();
        }
    }

    [ClientRpc]
    private void RemovePlayerClientRpc(ulong clientId) {
        if (IsServer) return;

        if (_playersInLobby.ContainsKey(clientId)) _playersInLobby.Remove(clientId);
        UpdateInterface();
    }

    [ClientRpc]
    private void ControlPanelClientRpc()
    {
        _mainLobbyScreen.gameObject.SetActive(false);
        _createScreen.gameObject.SetActive(false);
        _roomScreen.gameObject.SetActive(false);
    }

    public void OnReadyClicked() {
        SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetReadyServerRpc(ulong playerId) {
        _playersInLobby[playerId].isReady = !_playersInLobby[playerId].isReady;
        PropagateToClients();
        UpdateInterface();
    }
    public void ChangeTeamButton()
    {
        ChangeTeamServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeTeamServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (_playersInLobby.ContainsKey(clientId))
        {
            Debug.Log("Receieve Change Team!");
            if(_playersInLobby[clientId].isRedTeam)
            {
                Debug.Log("Change Blue");
                _playersInLobby[clientId].isRedTeam = false;
                _countTeam -= 1;
            }
            else
            {
                Debug.Log("Change Red");
                _playersInLobby[clientId].isRedTeam = true;
                _countTeam += 1;
            }
            
        }

        PropagateToClients();

        UpdateInterface();
    }

    public void SetKartIndex(int _idx)
    {
        SetKartServerRpc(NetworkManager.Singleton.LocalClientId, _idx);
        Debug.Log(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKartServerRpc(ulong _uid, int _idx)
    {
        Debug.Log("Set Kart" + _idx);
        _playersInLobby[_uid].KartIndex = _idx;
        _playersInLobby[_uid].position = PresetItem.KartType[_idx];
        PropagateToClients();
        UpdateInterface();
    }

    public void SetCharacterIndex(int _idx)
    {
        SetCharacterServerRpc(NetworkManager.Singleton.LocalClientId, _idx);
    }



    [ServerRpc(RequireOwnership = false)]
    private void SetCharacterServerRpc(ulong _uid, int _idx)
    {
        Debug.Log("Set Kart" + _idx);
        _playersInLobby[_uid].CharacterIndex = _idx;
        PropagateToClients();
        UpdateInterface();
    }

    public void UpdateInterface() {
        LobbyPlayersUpdated?.Invoke(_playersInLobby);
    }

    private async void OnLobbyLeft() {
        using (new Load("Leaving Lobby...")) {
            _playersInLobby.Clear();
            _countTeam = 0;
            await MatchmakingService.LeaveLobby();
        }
    }
    
    public override void OnDestroy() {
     
        base.OnDestroy();
        CreateLobbyScreen.LobbyCreated -= CreateLobby;
        LobbyRoomPanel.LobbySelected -= OnLobbySelected;
        RoomScreen.LobbyLeft -= OnLobbyLeft;
        RoomScreen.StartPressed -= OnGameStart;
        
        // We only care about this during lobby
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }
      
    }
    
    private async void OnGameStart() {
        using (new Load("Starting the game...")) {
            await MatchmakingService.LockLobby();
            ControlPanelClientRpc();
            
            NetworkManager.Singleton.SceneManager.LoadScene(_mapName, LoadSceneMode.Single);
        }
    }

    #endregion
}