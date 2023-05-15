using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetShowNickName : NetworkBehaviour
{

    public TextMeshPro Name;
    public GameObject[] TeamColors;
    public NetPlayerInfo npi;

    public void SetNameTack(string _name, int _teamNumber)
    {
        Name.text = _name;
        TeamColors[_teamNumber].SetActive(true);
    }
}
