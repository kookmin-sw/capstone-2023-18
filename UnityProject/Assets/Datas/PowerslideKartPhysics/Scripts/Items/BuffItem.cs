using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PowerslideKartPhysics
{
    public class BuffItem : Item
    {
        public float buffTime = 1f;

        [ServerRpc(RequireOwnership = false)]
        public override void ActivateServerRpc(ItemCastProperties props, ulong userid, ulong objectid)
        {
            base.ActivateServerRpc(props, userid, objectid);
            UseItem(userid, objectid, itemName);
        }

       

        private int getMyTeam(ulong id)
        {
            int myteamNum = 0;
            NetworkObject itemuser = GetNetworkObject(id);
            myteamNum = itemuser.GetComponent<NetPlayerInfo>().teamNumber.Value;
            return myteamNum;
        }

        private void UseItem(ulong userid, ulong objectid, string name)
        {
            List<playerData> tmp;
            int myteamNum = getMyTeam(objectid);
            if (myteamNum == 0)
            {
                tmp = ItemManager.instance.BlueTeam;
            }
            else tmp = ItemManager.instance.RedTeam;

            for (int i = 0; i < tmp.Count; i++)
            {
                ulong t_userid = tmp[i].clientId;
                ulong t_objid = tmp[i].networkobjectId;
                
                switch (name)
                {
                    case "ReverseBuffTeam":
                        UseReverseBuffTeamClientRpc(t_userid,t_objid);
                        break;
                    case "SlowItem":
                        UseSlowTeamClientRpc(t_userid,t_objid);
                        break;
                    case "ThunderItem" :
                        UseThunderItemClientRpc(t_userid,t_objid);
                        break;
                }
                
            }
        }

        #region Reverse
        
        [ClientRpc]
        private void UseReverseBuffTeamClientRpc(ulong userid, ulong objectid)
        {
            NetworkObject target = GetNetworkObject(objectid);
            target.GetComponentInChildren<ItemEffect>().EffectOn(ItemEffect.effectType.reverse,buffTime,userid);
            if (userid == NetworkManager.Singleton.LocalClientId)
            {
                NetKartInput playerInput = target.GetComponent<NetKartInput>();
                StartCoroutine(ReverseBuffTimer(buffTime,playerInput));
            }

        }
        
        
        IEnumerator ReverseBuffTimer(float bufftime, NetKartInput playerInput)
        {
            playerInput.isReverse = true;
            float currentTime = bufftime;
            while (currentTime > 0)
            {
                currentTime -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            playerInput.isReverse = false;
        }

        #endregion

        #region Slow

        [ClientRpc]
        private void UseSlowTeamClientRpc(ulong userid , ulong objectid)
        {
            NetworkObject target = GetNetworkObject(objectid);
            target.GetComponentInChildren<ItemEffect>().EffectOn(ItemEffect.effectType.slow,buffTime,userid);
            if (userid == NetworkManager.Singleton.LocalClientId)
            {
                StartCoroutine(SlowBuffTimer(buffTime, target.GetComponent<NetKartController>()));
            }
        }

        IEnumerator SlowBuffTimer(float bufftime, NetKartController playerController)
        {
            float currSpeed = playerController.speed;
            
            float currentTime = bufftime;
            while (currentTime > 0)
            {
                
                playerController.speed = currSpeed / 2;
                currentTime -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            playerController.speed = currSpeed;
        }

        #endregion

        #region Thunder

        [ClientRpc]
        private void UseThunderItemClientRpc(ulong userid, ulong objectid)
        {
            NetworkObject target = GetNetworkObject(objectid);
            target.GetComponentInChildren<ItemEffect>().EffectOn(ItemEffect.effectType.Thunder, buffTime, userid);
            if (userid == NetworkManager.Singleton.LocalClientId)
            {
                target.GetComponent<ItemCaster>().ImplementSpinServerRpc((int)ItemManager.SpinAxis.Pitch,3,userid);
            }
        }
        

        #endregion
        
    }
}

