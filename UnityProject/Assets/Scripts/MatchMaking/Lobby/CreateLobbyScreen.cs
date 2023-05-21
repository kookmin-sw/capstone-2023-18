using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyScreen : MonoBehaviour {
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Toggle _singleToggle;
    public static event Action<LobbyData> LobbyCreated;

    public void OnCreateClicked() {
        var lobbyData = new LobbyData {
            Name = _nameInput.text,
            isSinglePlay = _singleToggle.isOn,
            Map = _singleToggle.isOn ? "Kookmin" : "Kookmin_multi"
        };

        LobbyCreated?.Invoke(lobbyData);
    }
}

public struct LobbyData {
    public string Name;
    public bool isSinglePlay;
    public string Map;
}