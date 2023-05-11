using System;
using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using Unity.Netcode;
using UnityEngine;

public class ItemExplosion_effect : MonoBehaviour
{
    public float spinAmount = 3f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kart"))
        {
            ulong uid = other.GetComponent<NetworkObject>().OwnerClientId;
            other.GetComponent<ItemCaster>().ImplementSpinServerRpc((int)ItemManager.SpinAxis.Roll,spinAmount,uid);
        }
    }
}