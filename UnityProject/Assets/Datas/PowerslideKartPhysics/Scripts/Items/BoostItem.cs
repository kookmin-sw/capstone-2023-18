// Copyright (c) 2022 Justin Couch / JustInvoke
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace PowerslideKartPhysics
{
    // Class for boost item
    public class BoostItem : Item
    {
        public float boostAmount = 1.0f;
        public float boostTime = 2.0f;
        public bool isRush = false;

        

        [ServerRpc(RequireOwnership = false)]
        // Award boost to kart upon activation
        public override void ActivateServerRpc(ItemCastProperties props, ulong userid, ulong objectid) {
            base.ActivateServerRpc(props, userid, objectid);
            UseBoosterClientRpc(userid,objectid,isRush);
        }

        [ClientRpc]
        private void UseBoosterClientRpc(ulong userid,ulong objectid, bool isrush)
        {
            
            NetworkObject player = GetNetworkObject(objectid);
            ItemEffect itemEffect = player.GetComponentInChildren<ItemEffect>();

            if (player == null) return;    
          
            if (NetworkManager.Singleton.LocalClientId == userid )
            {
                if (IsClient)
                {
                    NetKartController itemowner = player.GetComponent<NetKartController>();
                    if (itemowner != null) StartCoroutine(itemowner.OnBooster(boostTime));
                    else Debug.Log("itemowner null");

                }
            }

            if (IsClient)
            {
                itemEffect.EffectOn(ItemEffect.effectType.boost, boostTime, userid);
                if (isrush)
                {
                    if(itemEffect != null )itemEffect.EffectOn(ItemEffect.effectType.rush, boostTime,userid);
                }
                //TODO : 피격무시 bool 처리 해주기
                StartCoroutine(player.GetComponent<NetKartController>().OnProtected(boostTime));

            }
            
            
        }
    }
}