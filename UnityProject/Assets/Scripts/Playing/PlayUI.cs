using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
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
    KartController UserKart;


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
        UserKart = GameObject.FindWithTag("Kart").GetComponent<KartController>();
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
        ITEM_ICONS = UI.transform.Find("ItemSlot/ICON").gameObject.GetComponent<ITEM>().ICONS;
    }

    // Update is called once per frame
    void Update()
    {
        //시간 업데이트
        ShowTime();
        //아이템 업데이트
        UpdateItem();
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
        TotalTime.text = "TIME : " + TransferTime(TimeCheck.nowTime);
        BestTime.text = "BEST : " + TransferTime(TimeCheck.BestTime);
    }

    //가지고 있는 아이템에 따라 UI 표시
    void UpdateItem()
    {
        IconImage.sprite = ITEM_ICONS[(int)UserKart.hasItem];
    }
}
