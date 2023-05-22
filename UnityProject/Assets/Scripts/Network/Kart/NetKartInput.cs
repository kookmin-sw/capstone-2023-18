using System;
using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using Unity.Netcode;
using UnityEngine;

public class NetKartInput : NetworkBehaviour
{
    public float Hmove;
    public float Vmove;
    public bool Drift;
    public bool Item;
    public bool Return;
    public bool isReverse = false;
    public bool isLimit = false;
    public ItemCaster caster;
    public NetPlayerInfo npi;
    private void Awake()
    {
        caster = GetComponent<ItemCaster>();
        npi = GetComponent<NetPlayerInfo>();
    }

    void Update()
    {
        if (IsOwner)
        {
            if (npi.isFinish.Value == 0)
            {
                if (isReverse)
                {
                    Hmove = Input.GetAxisRaw("Horizontal") * -1f;
                }
                else Hmove = Input.GetAxisRaw("Horizontal");

                Vmove = Input.GetAxisRaw("Vertical");
                Drift = Input.GetKey(KeyCode.LeftShift);

                if (Input.GetKey(KeyCode.R))
                {
                    Return = true;
                }

                if (Input.GetKeyDown(KeyCode.LeftControl) && !isLimit)
                {
                    if (caster != null)
                    {
                        caster.Cast(NetworkManager.Singleton.LocalClientId,GetComponent<NetworkObject>().NetworkObjectId);
                    }
                }
            }
            else
            {
                Hmove = Input.GetAxisRaw("Horizontal");
                Vmove = 0;
                Drift = Input.GetKey(KeyCode.LeftShift);
            }
        }

    }
}