using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetKartInput : NetworkBehaviour
{
    public float Hmove;
    public float Vmove;
    public bool Drift;
    public bool Item;
    public bool Return;

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
                Item = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Item = false;
            }
        }

    }
}
