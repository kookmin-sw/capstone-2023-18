using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : NetworkBehaviour
{
    [HideInInspector, Header("Component")]
    public GameObject UI;
    public TimeCheck TimeCheck;


    [Space, Header("Text")]
    public TextMeshProUGUI TotalTime;
    public TextMeshProUGUI BestTime;
    public TextMeshProUGUI Count;

    [Space, Header("Image")]
    public Image IconImage;
    public Sprite[] ITEM_ICONS;

    [HideInInspector]
    public KartController UserKart;


    private void Awake()
    {
        Init();
    }

    void Init()
    {
        UI = GameObject.Find("Play_UI");
        FindTextObj();
        LoadCoponent();
        LoadIconImages();
    }

    void LoadCoponent()
    {
        TimeCheck = GameObject.Find("StartingLIne").GetComponent<TimeCheck>();
        IconImage = UI.transform.Find("ItemSlot/ICON").gameObject.GetComponent<Image>();
    }

    void FindTextObj()
    {
        TotalTime = UI.transform.Find("TotalTime").GetComponent<TextMeshProUGUI>();
        BestTime = UI.transform.Find("BestTime").GetComponent<TextMeshProUGUI>();
        Count = UI.transform.Find("Count").GetComponent<TextMeshProUGUI>();
    }

    void LoadIconImages()
    {
        //ITEM_ICONS = UI.transform.Find("ItemSlot/ICON").gameObject.GetComponent<ITEM>().ICONS;
    }

    // Update is called once per frame
    void Update()
    {
        if (UserKart != null)
        {
            //�ð� ������Ʈ
            ShowTime();
            //������ ������Ʈ
            UpdateItem();
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
        TotalTime.text = "TIME : " + TransferTime(TimeCheck.nowTime);
        BestTime.text = "BEST : " + TransferTime(TimeCheck.BestTime);
    }

    //������ �ִ� �����ۿ� ���� UI ǥ��
    void UpdateItem()
    {
       
    }
}
