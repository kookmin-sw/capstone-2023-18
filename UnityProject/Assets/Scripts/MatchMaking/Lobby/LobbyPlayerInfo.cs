using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum PlayerPosition
{
    None = 0,
    Attack = 1,
    Defender = 2,
    Runner = 3
}

public class LobbyPlayerInfo : INetworkSerializable
{
    public bool isReady;//Ready 여부
    public bool isRedTeam;//RedTeam 여부
    public bool isHost;
    public PlayerPosition position;
    public int KartIndex;
    public int CharacterIndex;


    public LobbyPlayerInfo()
    {
        isReady = false;
        isRedTeam = false;
        isHost = false;
        position = PlayerPosition.None;
        KartIndex = 0;
        CharacterIndex = 0;
    }
    public LobbyPlayerInfo(bool _isReady, bool _isRedTeam, bool _isHost, PlayerPosition _position, int _KartIndex, int _CharacterIndex)
    {
        isReady = _isReady;
        isRedTeam = _isRedTeam;
        isHost = _isHost;
        position = _position;
        KartIndex = _KartIndex;
        CharacterIndex = _CharacterIndex;
    }
    

    // INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref isReady);
        serializer.SerializeValue(ref isRedTeam);
        serializer.SerializeValue(ref isHost);
        serializer.SerializeValue(ref position);
        serializer.SerializeValue(ref KartIndex);
        serializer.SerializeValue(ref CharacterIndex);
    }
    // ~INetworkSerializable
}
