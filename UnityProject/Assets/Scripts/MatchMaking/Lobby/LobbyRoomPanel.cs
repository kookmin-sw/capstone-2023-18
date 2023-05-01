using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyRoomPanel : MonoBehaviour {

    [SerializeField] private TMP_Text _nameText;

    public Lobby Lobby { get; private set; }

    public static event Action<Lobby> LobbySelected;

    public void Init(Lobby lobby) {
        UpdateDetails(lobby);
    }

    public void UpdateDetails(Lobby lobby) {
        Lobby = lobby;
        _nameText.text = lobby.Name;
    }

    public void Clicked() {
        LobbySelected?.Invoke(Lobby);
    }
}