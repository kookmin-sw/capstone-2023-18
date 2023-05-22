using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetPlayUI : NetworkBehaviour
{
    // Start is called before the first frame update
    //�÷��̾��� �⺻ ������ ����
    public NetKartInput input;
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
    public TextMeshProUGUI MyRank;

    [Space, Header("Image")]
    public Image IconImage;
    public Image[] RankImages;
    public Sprite[] ITEM_ICONS;
    public GameObject Warning;
    public GameObject TargetWarning;
    public GameObject limitImage;

    [Space, Header("Rank")]
    public ulong MyID = 0;
    public Transform[] Ranks;
    private string[] RankCount = { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th"};

    [Space, Header("End_UI")]
    public GameObject EndUI;
    public Transform MvpTransfrom;
    public TextMeshProUGUI Banner;
    public Image[] SourceImages; //00 ->Btn_red 01 ->Btn_blue
    public GameObject[] EndRanks;
    public TextMeshProUGUI[] EndRanks_ID;
    public TextMeshProUGUI[] EndRanks_Time;
    public Image[] EndRankds_BG;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        UI = GameObject.Find("Play_UI");
        EndUI = GameObject.Find("EndGame_UI");
        npm = gameObject.GetComponent<NetPlayManager>();
        FindTextObj();
        LoadCoponent();
        LoadIconImages();
    }


    public override void OnNetworkSpawn()
    {
        MyID = NetworkManager.Singleton.LocalClientId;
    }

    void LoadCoponent()
    {
        IconImage = UI.transform.Find("ItemSlot/ICON").gameObject.GetComponent<Image>();
    }

    void FindTextObj()
    {
        TotalTime = UI.transform.Find("TimeBG/TotalTime").GetComponent<Text>();
        BestTime = UI.transform.Find("TimeBG/BestTime").GetComponent<Text>();
        Lap = UI.transform.Find("TimeBG/Lap").GetComponent<Text>();
        Count = UI.transform.Find("Count").GetComponent<Text>();
        MyRank = UI.transform.Find("MyRank").GetComponent<TextMeshProUGUI>();
        Debug.Log("FIND COUNT" + Count.name);
        KMH = UI.transform.Find("KMH").GetComponent<Text>();

        Ranks = new Transform[8];
        RankImages = new Image[8];

        for(int i=1; i<=8; i++)
        {
            Ranks[i-1] = UI.transform.Find("Rank/"+i.ToString()).transform;
        }

        RankIds = new Text[Ranks.Length];
        for(int i=0; i<Ranks.Length; i++)
        {
            RankImages[i] = Ranks[i].GetComponent<Image>();
            RankIds[i] = Ranks[i].Find("name_Text").GetComponent<Text>();
        }
    }
   
    void LoadIconImages()
    {
        limitImage = UI.transform.Find("ItemSlot").Find("LockUI").gameObject;
        ITEM_ICONS = UI.transform.Find("ItemSlot/ICON").gameObject.GetComponent<ITEMIcon>().ICONS;
        TargetWarning = UI.transform.Find("TargetWarning").gameObject;
        Warning = UI.transform.Find("Warning").gameObject;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsClient)
        {
            //�ð� ������Ʈ
            ShowTime();
            //������ ������Ʈ
            UpdateItem();
            //��ŷ ������Ʈ
            UpdateRank();
            //�� ������Ʈ
            UpdateLap();
            //�ӵ�
            UpdateSpeed();
            UpdateLimit();
        }
    }

    
    
    void UpdateLimit()
    {
        if (npm.isStart.Value == true)
        {
            if (limitImage == null) return;
            if(input.isLimit) limitImage.SetActive(true);
            else limitImage.SetActive(false);
        }
       
    }
    //Play Time ����
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

    //������ �ִ� �����ۿ� ���� UI ǥ��
    void UpdateItem()
    {
        if (npm.isStart.Value == true)
        {
            IconImage.sprite = ITEM_ICONS[Player.Item.Value];
        }
    }

    //���� ������Ʈ
    void UpdateRank()
    {
        for(int i=0; i<8; i++)
        {
            if (i < npm.rank.Count)
            {
                ulong nowUser = npm.rank[i];
                if(nowUser == MyID)
                {
                    MyRank.text = RankCount[i];
                }

                Ranks[i].gameObject.SetActive(true);
                if(IsServer)
                {
                    //UpdateRankColorClientRpc(i, npm.Players[nowUser].teamNumber.Value);
                }
                RankIds[i].text = "USER  " + nowUser.ToString();
            }
            else
            {
                Ranks[i].gameObject.SetActive(false);
            }
        }
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    public void UpdateRankColorClientRpc(int _idx, int _teamNumber)
    {
        //RankImages[_idx].DOColor(_teamNumber == 0 ? Color.red : Color.blue, 0);
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

    //ī��Ʈ�ٿ� �޼��� ��ε�ĳ��Ʈ

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    public void CountdownClientRPC(int _count, ClientRpcParams rpcParams = default)
    {
        if(_count == 0)
        {
            Count.text = "";
        }
        else if(_count == -1)
        {
            Count.text = "GAME END";
        }
        else
        {
            Count.text = _count.ToString();
        }
    }

}
