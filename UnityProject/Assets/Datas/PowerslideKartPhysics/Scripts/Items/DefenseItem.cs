using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class DefenseItem : Item
{
    public float defenseTime = 1f;
    
    [ServerRpc(RequireOwnership = false)]
    public override void ActivateServerRpc(ItemCastProperties props, ulong userid, ulong objectid)
    {
        base.ActivateServerRpc(props, userid,objectid);
        UseDefense(userid,objectid);
    }

    private void UseDefense(ulong userid, ulong objectid)
    {
        NetworkObject player = GetNetworkObject(objectid);
        NetPlayerInfo info = player.GetComponent<NetPlayerInfo>();
        if (info.myPosition.Value == (int)PlayerPosition.Defender)
        {
            List<playerData> tmp;
            if (info.teamNumber.Value == 0) tmp = ItemManager.instance.RedTeam;
            else tmp = ItemManager.instance.BlueTeam;
            for (int i = 0; i < tmp.Count; i++)
            {
                ulong t_userid = tmp[i].clientId;
                ulong t_objid = tmp[i].networkobjectId;
                UseDefenseItemClientRpc(t_userid, t_objid, true);
            }
        }
        else
        {
            UseDefenseItemClientRpc(userid,objectid,false);
        }
    }
    
    //Self Defense
    [ClientRpc]
    private void UseDefenseItemClientRpc(ulong userid, ulong objectid,bool isDefender)
    {
        NetworkObject player = GetNetworkObject(objectid);
        if (NetworkManager.Singleton.LocalClientId == userid) 
        {
            //TODO : bool 형태로 구현해야함 추후 피격과 같이 구현 예정
            NetKartController nkc = player.GetComponent<NetKartController>(); 
            StartCoroutine(nkc.OnProtected(defenseTime));
        }
        //effect로만 구현
        ItemEffect itemEffect = player.GetComponentInChildren<ItemEffect>();
        if (isDefender)
        {
            itemEffect.EffectOn(ItemEffect.effectType.TeamShield,defenseTime,userid);
        }
        else
        {
            itemEffect.EffectOn(ItemEffect.effectType.shield,defenseTime,userid);
        }
        
        
        
    }
}
