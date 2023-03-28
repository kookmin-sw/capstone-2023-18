using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetKartInput : NetworkBehaviour
{
    public NetworkVariable<float> Hmove = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> Vmove = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> Drift = new NetworkVariable<bool>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> Item = new NetworkVariable<bool>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> Return = new NetworkVariable<bool>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            Hmove.Value = Input.GetAxisRaw("Horizontal");
            Vmove.Value = Input.GetAxisRaw("Vertical");
            Drift.Value = Input.GetKey(KeyCode.LeftShift);
            if (Input.GetKey(KeyCode.R))
            {
                Return.Value = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !PlayManager.isReturning)
            {
                Item.Value = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Item.Value = false;
            }
        }

    }
}
