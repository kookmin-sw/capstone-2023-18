using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CreateLobbyScreen : MonoBehaviour {
    [SerializeField] private TMP_InputField _nameInput;

    public static event Action<LobbyData> LobbyCreated;

    public void OnCreateClicked() {
        var lobbyData = new LobbyData {
            Name = _nameInput.text,
        };

        LobbyCreated?.Invoke(lobbyData);
    }
}

public struct LobbyData {
    public string Name;
}