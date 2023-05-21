using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayerInfoComparer : IComparer<NetPlayerInfo>
{
    public int Compare(NetPlayerInfo x, NetPlayerInfo y)
    {
        //완주 했는지? 했으면 시간순 비교
        if(y.isFinish.Value == x.isFinish.Value)
        {
            if (y.isFinish.Value == 0)
            {
                int lapComparison = y.Lap.Value.CompareTo(x.Lap.Value);
                if (lapComparison != 0) return -lapComparison;

                int rpNumComparison = y.RpNum.Value.CompareTo(x.RpNum.Value);
                if (rpNumComparison != 0) return -rpNumComparison;

                return -y.CheckPointDistance.Value.CompareTo(x.CheckPointDistance.Value);
            }
            else
            {
                int timeComparison = y.LapTimes.Value.CompareTo(x.LapTimes.Value);
                if (timeComparison != 0) return timeComparison;
            }
        }
        else
        {
            return x.isFinish.Value > y.isFinish.Value ? 1 : -1;
        }

        return 0;
        

    }
}
