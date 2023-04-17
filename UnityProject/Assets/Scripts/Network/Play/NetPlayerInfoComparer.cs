using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayerInfoComparer : IComparer<NetPlayerInfo>
{
    public int Compare(NetPlayerInfo x, NetPlayerInfo y)
    {
        int lapComparison = y.Lap.Value.CompareTo(x.Lap.Value);
        if (lapComparison != 0) return -lapComparison;

        int rpNumComparison = y.RpNum.Value.CompareTo(x.RpNum.Value);
        if (rpNumComparison != 0) return -rpNumComparison;

        return -y.CheckPointDistance.Value.CompareTo(x.CheckPointDistance.Value);

    }
}
