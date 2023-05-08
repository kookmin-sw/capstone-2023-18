using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using Unity.Netcode;
using UnityEngine;

public class DefenseItem : Item
{
    public float defenseTime = 1f;
    
    [ServerRpc(RequireOwnership = false)]
    public override void ActivateServerRpc(ItemCastProperties props, ulong userid, ulong objectid)
    {
        base.ActivateServerRpc(props, userid,objectid);
        UseDefenseItemClientRpc(userid, objectid);
        
    }

    
    //Self Defense
    [ClientRpc]
    private void UseDefenseItemClientRpc(ulong userid, ulong objectid)
    {

        NetworkObject player = GetNetworkObject(objectid);
        
        if (NetworkManager.Singleton.LocalClientId == userid)
        {
            
            //TODO : bool 형태로 구현해야함 추후 피격과 같이 구현 예정
            
            
            
        }
        
        //effect로만 구현
        ItemEffect itemEffect = player.GetComponentInChildren<ItemEffect>();
        itemEffect.EffectOn(ItemEffect.effectType.shild,defenseTime,userid);
    }
}
