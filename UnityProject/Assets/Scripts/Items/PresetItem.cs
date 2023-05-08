using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentItems
{
    NONE = 0,
    KART = 1,
    CHARACTER = 2
}
public class PresetItem : MonoBehaviour
{
    public static Dictionary<int, PlayerPosition> KartType = new Dictionary<int, PlayerPosition>()
{
    {0, PlayerPosition.Defender}, {1, PlayerPosition.Defender}, {2, PlayerPosition.Defender}, {3, PlayerPosition.Defender},
    {4, PlayerPosition.Attack}, {5, PlayerPosition.Attack}, {6, PlayerPosition.Attack}, {7, PlayerPosition.Attack},
    {8, PlayerPosition.Runner}, {9, PlayerPosition.Runner}, {10, PlayerPosition.Runner}, {11, PlayerPosition.Runner}
};
    public EquipmentItems type;
    public PlayerPosition Position;
}
