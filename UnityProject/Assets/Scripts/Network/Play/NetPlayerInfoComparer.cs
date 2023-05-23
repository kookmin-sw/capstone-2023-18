using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayerInfoComparer : IComparer<NetPlayerInfo>
{
    public int Compare(NetPlayerInfo x, NetPlayerInfo y)
    {
        // 완주 비교
        if(y.isFinish.Value == x.isFinish.Value)
        {
            // 둘 다 완주를 못했다면
            if (y.isFinish.Value == 0)
            {
                //바퀴수가 많은쪽이 우선
                int lapComparison = y.Lap.Value.CompareTo(x.Lap.Value);
                if (lapComparison != 0) return -lapComparison;

                //직선 구간을 많이 통과한 쪽이 우선
                int rpNumComparison = y.RpNum.Value.CompareTo(x.RpNum.Value);
                if (rpNumComparison != 0) return -rpNumComparison;

                //같은 직선구간이라면 얼마나 더 멀리와있는지(내적값)
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
            //완주 한 쪽이 우선 순위
            return x.isFinish.Value > y.isFinish.Value ? 1 : -1;
        }

        return 0;
        

    }
}
