using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ReturnToKart : NetworkBehaviour
{
    NetKartInput input;
    PlayCheckPoint playcheckpoint;

    void Start()
    {
        if (IsOwner)
        {
            input = gameObject.GetComponent<NetKartInput>();
            playcheckpoint = GameObject.Find("@PlayManager").GetComponent<PlayCheckPoint>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        /***
        if(IsOwner && input.Return)
        {
            
            input.Return = false;
            StartCoroutine(playcheckpoint.ReturnToCP());
        }
        ***/
    }
}
