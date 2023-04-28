using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPanel : MonoBehaviour {
    [SerializeField] private Text _nameText, _statusText, _isHost;
    [SerializeField] private bool isRedTeam;

    public ulong PlayerId { get; private set; }

    public void Init(ulong playerId) {
        PlayerId = playerId;
        _nameText.text = $"Player {playerId}";
    }

    public void SetReady(bool isReady) {
        _statusText.text = isReady ? "Ready" : "";
        _statusText.color = Color.green;
    }

}