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
    public PlayerPosition position;

    public LobbyPlayerInfo()
    {
        isReady = false;
        isRedTeam = false;
        position = PlayerPosition.None;
    }
    public LobbyPlayerInfo(bool _isReady, bool _isRedTeam, PlayerPosition _position)
    {
        isReady = _isReady;
        isRedTeam = _isRedTeam;
        position = _position;
    }
    

    // INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref isReady);
        serializer.SerializeValue(ref isRedTeam);
        serializer.SerializeValue(ref position);
    }
    // ~INetworkSerializable
}
