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
    public ItemCaster caster;
    private void Awake()
    {
        caster = GetComponent<ItemCaster>();
    }

    void Update()
    {
        if (IsOwner)
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

            if (Input.GetKeyDown(KeyCode.LeftControl) && !PlayManager.isReturning)
            {
                if (caster != null) {
                    Debug.Log(NetworkObjectId);
                    caster.CastServerRpc(NetworkManager.Singleton.LocalClientId,NetworkObjectId);
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Item = false;
            }
        }
        
        
    }
}   
