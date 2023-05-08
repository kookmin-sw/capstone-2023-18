using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    Button[] Button_Characters;
    LobbyOrchestrator LO;
    void Start()
    {
        Button_Characters = gameObject.GetComponentsInChildren<Button>();
        LO = LobbyOrchestrator.Instance.GetComponent<LobbyOrchestrator>();

        for (int i = 0; i < Button_Characters.Length; i++)
        {
            int tmp = i;
            Button_Characters[i].onClick.AddListener(() => LO.SetCharacterIndex(tmp));
        }
    }
}
