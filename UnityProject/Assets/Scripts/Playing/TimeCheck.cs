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

    public int Lap;
    // Start is called before the first frame update
    void Start()
    {
        Lap = 0;
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
        if(other.CompareTag("Kart") && Lap > 0)
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Kart"))
        {
            Lap += 1;
        }
    }


}
