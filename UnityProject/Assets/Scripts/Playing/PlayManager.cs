using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    static PlayManager s_Instance;
    public static PlayManager Instace { get { Init(); return s_Instance; } }

    //게임 시작했는지?
    public static bool isStart;
    public static float StartTime;

    //플레이 진행도 체크
    public static int Lap; // 몇 바퀴 진행중인지.
    public static int MaxLap; // 몇 바퀴 맵인지.
    public static int CP; // 현재 최대로 진행된 CheckPoint
    public static int MaxCP; // 맵에 존재하는 최대 CheckPoint 개수.
    public static CP[] nextCP;

    public static bool isReturning;

    static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("@PlayManager");

            if (go == null)
            {
                //만약 Manager Object를 생성 한 적이 없다.
                go = new GameObject { name = "@PlayManager" };
                go.AddComponent<PlayUI>();
                go.AddComponent<CountDown>();
                go.AddComponent<PlayCheckPoint>();
            };
            s_Instance = go.GetComponent<PlayManager>();
        }
        isStart = false;
        isReturning = false;
        StartTime = 3f;
    }


    void Awake()
    {
        Init();
    }

    private void Update()
    {
        if(Lap == MaxLap)
        {
            Debug.Log("FINISH");
        }
    }

}
