using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     NetworkBehaviours cannot easily be parented, so the network logic will take place
///     on the network scene object "NetworkLobby"
/// </summary>
public class RoomScreen : MonoBehaviour {
    [SerializeField] private LobbyPlayerPanel[] _redPlayers;
    [SerializeField] private LobbyPlayerPanel[] _bluePlayers;
    [SerializeField] private TMP_Text _waitingText;
    [SerializeField] private GameObject _startButton, _readyButton;

    private readonly List<LobbyPlayerPanel> _playerPanels = new();
    private bool _allReady;
    private bool _ready;

    public static event Action StartPressed; 

    private void OnEnable() {

        LobbyOrchestrator.LobbyPlayersUpdated += NetworkLobbyPlayersUpdated;
        //MatchmakingService.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;
        _startButton.SetActive(false);
        _readyButton.SetActive(true);

        _ready = false;
    }

    private void OnDisable() {
        LobbyOrchestrator.LobbyPlayersUpdated -= NetworkLobbyPlayersUpdated;
        //MatchmakingService.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
    }

    public static event Action LobbyLeft;

    public void OnLeaveLobby() {
        LobbyLeft?.Invoke();
    }

    private void NetworkLobbyPlayersUpdated(Dictionary<ulong, LobbyPlayerInfo> players) {
        var allActivePlayerIds = players.Keys;

        int redCount = 0, blueCount = 0;
        foreach (var player in players)
        {
            Debug.Log("User TEST" + player.Key + " " + player.Value.KartIndex + " " + player.Value.CharacterIndex);
            var currentPanel = _playerPanels.FirstOrDefault(p => p.PlayerId == player.Key);
            if (player.Value.isRedTeam)
            {
                _redPlayers[redCount].gameObject.SetActive(true);
                _redPlayers[redCount].Init(player.Key);
                _redPlayers[redCount].SetItem(player.Value.KartIndex, player.Value.CharacterIndex);
                _redPlayers[redCount].SetHost(player.Value.isHost);
                _redPlayers[redCount].SetYour(player.Key == NetworkManager.Singleton.LocalClientId);
                _redPlayers[redCount++].SetReady(player.Value.isReady);
            }
            else
            {
                _bluePlayers[blueCount].gameObject.SetActive(true);
                _bluePlayers[blueCount].Init(player.Key);
                _bluePlayers[blueCount].SetItem(player.Value.KartIndex, player.Value.CharacterIndex);
                _bluePlayers[blueCount].SetHost(player.Value.isHost);
                _bluePlayers[blueCount].SetYour(player.Key == NetworkManager.Singleton.LocalClientId);
                _bluePlayers[blueCount++].SetReady(player.Value.isReady);
            }

        }

        // Remove all inactive panels
        for(; redCount < _redPlayers.Length; redCount++)
        {
            _redPlayers[redCount].gameObject.SetActive(false);
        }

        for (; blueCount < _bluePlayers.Length; blueCount++)
        {
            _bluePlayers[blueCount].gameObject.SetActive(false);
        }


        _startButton.SetActive(NetworkManager.Singleton.IsHost && players.All(p => p.Value.isReady));
        //_readyButton.SetActive(!_ready);
    }

    private void OnCurrentLobbyRefreshed(Lobby lobby) {
        _waitingText.text = $"Waiting on players... {lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    public void OnReadyClicked() {
        //_readyButton.SetActive(false);
        _ready = !_ready;
    }

    public void OnStartClicked() {
        StartPressed?.Invoke();
    }
}