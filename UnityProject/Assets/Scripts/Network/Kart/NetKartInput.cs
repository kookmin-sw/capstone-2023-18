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
    public ItemCaster caster;
    private void Awake()
    {
        caster = GetComponent<ItemCaster>();
    }

    void Update()
    {
        if (IsOwner)
        {
            Hmove = Input.GetAxisRaw("Horizontal");
            Vmove = Input.GetAxisRaw("Vertical");
            Drift = Input.GetKey(KeyCode.LeftShift);
            if (Input.GetKey(KeyCode.R))
            {
                Return = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !PlayManager.isReturning)
            {
                NetPressItem();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Item = false;
            }
        }
        
        
    }
    
    protected void NetPressItem() {
        if (caster != null) {
            caster.Cast();
        }
    }
}   
