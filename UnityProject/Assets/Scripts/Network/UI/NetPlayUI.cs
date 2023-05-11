using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetPlayUI : NetworkBehaviour
{
    // Start is called before the first frame update
    //플레이어의 기본 정보를 담음
    public NetPlayerInfo Player;
    public NetPlayManager npm;

    [HideInInspector, Header("Component")]
    public GameObject UI;
    public TimeCheck TimeCheck;

    
    [Space, Header("Text")]
    public Text TotalTime;
    public Text BestTime;
    public Text Count;
    public Text[] RankIds;
    public Text Lap;
    public Text KMH;

    [Space, Header("Image")]
    public Image IconImage;
    public Sprite[] ITEM_ICONS;
    public GameObject Warning;

    [Space, Header("Rank")]
    public Transform[] Ranks;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        UI = GameObject.Find("Play_UI");
        npm = gameObject.GetComponent<NetPlayManager>();
        FindTextObj();
        LoadCoponent();
        LoadIconImages();
    }

    private void FixedUpdate()
    {
        //MatchmakingService.showRoomName();
    }

    void LoadCoponent()
    {
        IconImage = UI.transform.Find("ItemSlot/ICON").gameObject.GetComponent<Image>();
    }

    void FindTextObj()
    {
        TotalTime = UI.transform.Find("TotalTime").GetComponent<Text>();
        BestTime = UI.transform.Find("BestTime").GetComponent<Text>();
        Count = UI.transform.Find("Count").GetComponent<Text>();
        Debug.Log("FIND COUNT" + Count.name);
        KMH = UI.transform.Find("KMH").GetComponent<Text>();

        Ranks = new Transform[8];

        for(int i=1; i<=8; i++)
        {
            Ranks[i-1] = UI.transform.Find("Rank/"+i.ToString()).transform;
        }

        RankIds = new Text[Ranks.Length];
        for(int i=0; i<Ranks.Length; i++)
        {
            RankIds[i] = Ranks[i].Find("name_Text").GetComponent<Text>();
        }
        Lap = UI.transform.Find("Lap").GetComponent<Text>();
    }

    void LoadIconImages()
    {
        ITEM_ICONS = UI.transform.Find("ItemSlot/ICON").gameObject.GetComponent<ITEM>().ICONS;
        Warning = UI.transform.Find("Warning").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClient)
        {
            //시간 업데이트
            ShowTime();
            //아이템 업데이트
            UpdateItem();
            //랭킹 업데이트
            UpdateRank();
            //랩 업데이트
            UpdateLap();
            //속도
            UpdateSpeed();
        }
    }


    //Play Time 측정
    string TransferTime(float t)
    {
        return string.Format("{0:0}:{1:00}.{2:000}",
                     Mathf.Floor(t / 60),//minutes
                     Mathf.Floor(t) % 60,//seconds
                     Mathf.Floor((t * 1000) % 1000));//miliseconds
    }

    void ShowTime()
    {
        if (npm.isStart.Value == true)
        {
            TotalTime.text = "TIME : " + TransferTime(npm.PlayTime.Value);
            BestTime.text = "BEST : " + TransferTime(Player.BestTime.Value);
        }
    }

    //가지고 있는 아이템에 따라 UI 표시
    void UpdateItem()
    {
        if (npm.isStart.Value == true)
        {
            IconImage.sprite = ITEM_ICONS[Player.Item.Value];
        }
    }

    //순위 업데이트
    void UpdateRank()
    {
        for(int i=0; i<8; i++)
        {
            if (i < npm.rank.Count)
            {
                Ranks[i].gameObject.SetActive(true);
                RankIds[i].text = "Unknown" + npm.rank[i].ToString();
            }
            else
            {
                Ranks[i].gameObject.SetActive(false);
            }
        }
    }

    void UpdateLap()
    {
        if (npm.isStart.Value == true)
        {
            Lap.text = $"LAP : {Player.Lap.Value} / {npm.MaxLap}";
        }
    }

    void UpdateSpeed()
    {
        if (npm.isStart.Value == true)
        {
            KMH.text = Player.KMH.Value.ToString();
        }
    }

    //카운트다운 메세지 브로드캐스트

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    public void CountdownClientRPC(int _count, ClientRpcParams rpcParams = default)
    {
        if(_count == 0)
        {
            Count.text = "";
        }
        else
        {
            Count.text = _count.ToString();
        }
    }

}
