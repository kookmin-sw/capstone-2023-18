using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ResetLobby : MonoBehaviour
{
    void Start()
    {
        GameObject lo = GameObject.Find("Lobby Orchestrator");
        GameObject cl = GameObject.Find("CheckLoading");
        GameObject nm = GameObject.Find("Network Manager");

        if (lo != null)
        {
            Destroy(lo);
        }

        if (cl != null)
        {
            Destroy(cl);
        }

        if(nm != null)
        {
            Destroy(nm);
        }

    }
}
