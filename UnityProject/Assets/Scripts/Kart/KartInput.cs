using System;
using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;

public class KartInput : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if (!CompareTag("AI"))
        {
            
            Hmove = Input.GetAxisRaw("Horizontal");
            Vmove = Input.GetAxisRaw("Vertical");
            Drift = Input.GetKey(KeyCode.LeftShift);
            if(Input.GetKey(KeyCode.R))
            {
                Return = true;
            }

            if(Input.GetKeyDown(KeyCode.LeftControl) && !PlayManager.isReturning)
            {
                PressItem();
            }
            else if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                Item = false;
            }
        }

    }
    protected void PressItem() {
        
    }
    
   
}
