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
        
        public override void Activate(ItemCastProperties props, ulong userid, ulong objectid)
        {
            base.Activate(props, userid, objectid);
            useItem(itemName, userid);
        }

        private void useItem(string itemName, ulong userid)
        {
            switch (itemName)
            {
                case "ReverseBuffother" :
                    UseReverseBuffotherClientRpc(userid);
                    break;
                case "ReverseBuffTeam" :
                    break;
            }
        }

        [ClientRpc]
        private void UseReverseBuffotherClientRpc(ulong userid)
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
        
    }
}

