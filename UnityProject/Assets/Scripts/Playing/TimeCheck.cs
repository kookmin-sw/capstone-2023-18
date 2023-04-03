using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCheck : MonoBehaviour
{
    public float nowTime;
    public float LastTime;
    public float BestTime;
    public List<float> LapTime;
    public bool isStart;

    void Start()
    {
        init();
    }

    void init()
    {
        PlayManager.Lap = 0;
        PlayManager.MaxLap = 3;

        nowTime = 0;
        LastTime = 0;
        BestTime = 0;
        isStart = false;
    }

    private void FixedUpdate()
    {
        if(PlayManager.isStart)
        {
            nowTime += Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kart") && PlayManager.CP == (PlayManager.MaxCP - 1))
        {
            float TotalTime = nowTime;
            float NowLap = TotalTime - LastTime;
            
            LapTime.Add(NowLap);
            
            LastTime = TotalTime;
            if (BestTime == 0)
            {
                BestTime = NowLap;
            }
            else
            {
                BestTime = NowLap < BestTime ? NowLap : BestTime;
            }
            PlayManager.Lap += 1;
        }
    }
}
