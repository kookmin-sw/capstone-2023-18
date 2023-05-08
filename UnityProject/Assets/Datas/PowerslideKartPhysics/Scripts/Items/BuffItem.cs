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
            Debug.Log(itemName);
            useItem(itemName, userid, objectid);
        }

        private void useItem(string itemName, ulong userid, ulong objectid)
        {
            switch (itemName)
            {
                case "ReverseBuffother" :
                    UseReverseBuffotherClientRpc(userid,objectid);
                    break;
                case "ReverseBuffTeam" :
                    UseReverseBuffTeamClientRpc(userid,getMyTeam(objectid));
                    break;
                case "SlowItem" : 
                    UseSlowTeamClientRpc(getMyTeam(objectid),userid);
                    break;
            }
        }

        private int getMyTeam(ulong id)
        {
            int myteamNum = 0;
            
            NetworkObject itemuser = GetNetworkObject(id);
            myteamNum = itemuser.GetComponent<NetPlayerInfo>().teamNumber.Value;
            return myteamNum;
        }
        
        [ClientRpc]
        private void UseReverseBuffTeamClientRpc(ulong userid, int teamNum)
        {
            Debug.Log("check" + teamNum);
            List<playerData> data = ItemManager.instance.PlayerDatas;
            for (int i = 0; i < data.Count; i++)
            {
                playerData tmp = data[i];
                //use to not my team
                if (tmp.teamNumber != teamNum)
                {
                    NetworkObject target = GetNetworkObject(tmp.networkobjectId);
                    target.GetComponentInChildren<ItemEffect>().EffectOn(ItemEffect.effectType.reverse,buffTime,userid);
                    if (tmp.clientId == NetworkManager.Singleton.LocalClientId)
                    {
                        NetKartInput playerInput = target.GetComponent<NetKartInput>();
                        StartCoroutine(ReverseBuffTimer(buffTime,playerInput));
                    }
                }
            }

        }
        
        [ClientRpc]
        private void UseReverseBuffotherClientRpc(ulong userid, ulong objectid)
        {
            if (NetworkManager.Singleton.LocalClientId != userid)
            {
                GameObject player = NetworkManager.LocalClient.PlayerObject.gameObject;
                NetKartInput playerInput = player.GetComponent<NetKartInput>();
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

        [ClientRpc]
        private void UseSlowTeamClientRpc(int teamNum,ulong userid)
        {
            Debug.Log("check");   
            List<playerData> data = ItemManager.instance.PlayerDatas;
            for (int i = 0; i < data.Count; i++)
            {
                playerData tmp = data[i];
                //use to not my team
                if (tmp.teamNumber != teamNum)
                {
                    NetworkObject target = GetNetworkObject(tmp.networkobjectId);
                    Debug.Log("targetId : " + tmp.networkobjectId);
                    target.GetComponentInChildren<ItemEffect>().EffectOn(ItemEffect.effectType.slow,buffTime,userid);
                    if (tmp.clientId == NetworkManager.Singleton.LocalClientId)
                    {
                        
                        StartCoroutine(SlowBuffTimer(buffTime, target.GetComponent<NetKartController>()));
                    }
                }
            }
        }

        IEnumerator SlowBuffTimer(float bufftime, NetKartController playerController)
        {
            float currSpeed = playerController.MaxSpeed;
            
            float currentTime = bufftime;
            while (currentTime > 0)
            {
                
                playerController.MaxSpeed = currSpeed / 2;
                currentTime -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            playerController.MaxSpeed = currSpeed;
        }
    }
}

