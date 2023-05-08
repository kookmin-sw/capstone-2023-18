using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SelectKart : MonoBehaviour
{
    Button[] Button_Karts;
    LobbyOrchestrator LO;
    void Start()
    {
        Button_Karts = gameObject.GetComponentsInChildren<Button>();
        LO = LobbyOrchestrator.Instance.GetComponent<LobbyOrchestrator>();

        for (int i=0; i<Button_Karts.Length; i++)
        {
            int temp = i;
            Button_Karts[i].onClick.AddListener(() => LO.SetKartIndex(temp));
        }
    }
}
