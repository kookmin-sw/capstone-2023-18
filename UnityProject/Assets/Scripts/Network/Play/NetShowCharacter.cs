using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetShowCharacter : NetworkBehaviour
{
    PresetItem[] Characters;
    NetPlayerInfo npi;
    // Start is called before the first frame update
    void Awake()
    {
        Characters = gameObject.GetComponentsInChildren<PresetItem>();
        npi = gameObject.GetComponentInParent<NetPlayerInfo>();
        SetCharacter(-1);
    }

    public void SetCharacter(int _idx)
    {
        for(int i=0; i<Characters.Length; i++)
        {
            Characters[i].gameObject.SetActive(i == _idx ? true : false);
        }
    }

    public void Update()
    {
        if(IsSpawned)
        {
            Characters[npi.myCharacter.Value].gameObject.SetActive(true);
        }
    }
}
