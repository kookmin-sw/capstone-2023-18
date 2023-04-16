using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayerInfoComparer : IComparer<NetPlayerInfo>
{
    public int Compare(NetPlayerInfo x, NetPlayerInfo y)
    {
        int distanceComparison = y.CheckPointDistance.Value.CompareTo(x.CheckPointDistance.Value);
        if (distanceComparison != 0) return -distanceComparison;

        int rpNumComparison = y.RpNum.Value.CompareTo(x.RpNum.Value);
        if (rpNumComparison != 0) return -rpNumComparison;

        return -y.Lap.Value.CompareTo(x.Lap.Value);
    }
}
