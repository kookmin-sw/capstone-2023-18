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
        public float currSpeed = 21500;

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
                    case "LimitSkillItem" : 
                        UseLimitSkillClientRpc(t_userid,t_objid);
                        break;
                    case "SquidItem" : 
                        UseSquidItemClientRpc(t_userid,t_objid);
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
                target.GetComponent<ItemCaster>().ImplementSpinServerRpc((int)ItemManager.SpinAxis.Yaw,3,userid);
            }
        }
        

        #endregion

        #region LimitSkill

        [ClientRpc]
        private void UseLimitSkillClientRpc(ulong userid , ulong objectid)
        {
            NetworkObject target = GetNetworkObject(objectid);
            //target.GetComponentInChildren<ItemEffect>().EffectOn(ItemEffect.effectType.slow,buffTime,userid);
            if (userid == NetworkManager.Singleton.LocalClientId)
            {
                StartCoroutine(LimitSkillBuffTimer(buffTime, target.GetComponent<NetKartInput>()));
            }
        }

        IEnumerator LimitSkillBuffTimer(float bufftime, NetKartInput netKartInput)
        {
            netKartInput.isLimit = true;
            float currentTime = bufftime;
            while (currentTime > 0)
            {
                currentTime -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            netKartInput.isLimit = false;
        }

        #endregion

        #region Squid
        
        [ClientRpc]
        private void UseSquidItemClientRpc(ulong userid, ulong objectid)
        {
            NetworkObject target = GetNetworkObject(objectid);
            target.GetComponentInChildren<ItemEffect>().EffectOn(ItemEffect.effectType.Squid,buffTime,userid);
            if (userid == NetworkManager.Singleton.LocalClientId)
            {
                //TODO : UI Effect
                StartCoroutine(oilTimer(2f));

            }

        }
        IEnumerator oilTimer(float bufftime)
        {
            GameObject oil = GameObject.Find("Play_UI/ItemEffect/Squid");
            
            oil.SetActive(true);
            float currentTime = bufftime;
            while (currentTime > 0)
            {
                currentTime -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            oil.SetActive(false);
        }
        
        
        

        #endregion
    }
}

