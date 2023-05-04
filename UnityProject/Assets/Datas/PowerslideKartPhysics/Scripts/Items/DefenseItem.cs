using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using Unity.Netcode;
using UnityEngine;

public class DefenseItem : Item
{
    public float defenseTime = 1f;
    
    public override void Activate(ItemCastProperties props, ulong userid, ulong objectid)
    {
        base.Activate(props, userid,objectid);
        UseDefenseItemClientRpc(userid);
        
    }

    
    //Self Defense
    [ClientRpc]
    private void UseDefenseItemClientRpc(ulong userid)
    {
        if (NetworkManager.Singleton.LocalClientId == userid)
        {
            GameObject player = NetworkManager.LocalClient.PlayerObject.gameObject;
            //TODO : bool 형태로 구현해야함 추후 피격과 같이 구현 예정
            
            
            //effect로만 구현
            ItemEffect itemEffect = player.GetComponentInChildren<ItemEffect>();
            itemEffect.EffectOnClientRpc(ItemEffect.effectType.shild,defenseTime,userid);
        }
    }
}
