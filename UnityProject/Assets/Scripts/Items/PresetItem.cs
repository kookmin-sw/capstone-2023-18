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
    {4, PlayerPosition.Runner}, {5, PlayerPosition.Runner}, {6, PlayerPosition.Runner}, {7, PlayerPosition.Runner},
    {8, PlayerPosition.Attack}, {9, PlayerPosition.Attack}, {10, PlayerPosition.Attack}, {11, PlayerPosition.Attack}
};
    public EquipmentItems type;
    public PlayerPosition Position;
}
