using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerPanel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _nameText, _statusText, _positionText;
    [SerializeField] private GameObject _isHost, _isYour;
    [SerializeField] private bool isRedTeam;
    public LobbyPlayerItem SelectItem;

    public ulong PlayerId { get; private set; }

    public void Init(ulong playerId) {
        PlayerId = playerId;
        _nameText.text = $"Player {playerId}";
    }

    public void SetReady(bool isReady) {
        _statusText.text = isReady ? "Ready" : "";
        _statusText.color = Color.green;
    }

    public void SetItem(int kartidx, int characteridx)
    {
        SelectItem.KartIndex = kartidx;
        SelectItem.CharacterIndex = characteridx;
    }

    public void SetHost(bool isHost)
    {
        _isHost.SetActive(isHost);
    }

    public void SetYour(bool isYour)
    {
        _isYour.SetActive(isYour);
    }
}