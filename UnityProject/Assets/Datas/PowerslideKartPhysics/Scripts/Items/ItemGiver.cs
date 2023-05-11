// Copyright (c) 2022 Justin Couch / JustInvoke
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace PowerslideKartPhysics
{
    [DisallowMultipleComponent]
    // Class for objects that give items to karts when touched
    public class ItemGiver : NetworkBehaviour
    {
        ItemManager manager;
        Collider trig;
        Renderer rend;
        public string itemName;
        public int ammo = 1;
        public float cooldown = 5.0f;
        float offTime = 0.0f;

        private void Awake() {
            manager = FindObjectOfType<ItemManager>();
            trig = GetComponent<Collider>();
            rend = GetComponent<Renderer>();
            offTime = cooldown;
        }

        private void Update() {
            if (trig == null || rend == null) { return; }

            offTime += Time.deltaTime;

            // Disable trigger and renderer during cooldown
            trig.enabled = rend.enabled = offTime >= cooldown;
        }

        private void OnTriggerEnter(Collider other) {
            if (manager != null) {
                // Give item to caster

                //해당 클라이더만
                if (!other.CompareTag("Kart")) return;
                ItemCaster caster = other.transform.GetTopmostParentComponent<ItemCaster>();
                if (caster != null) {
                    offTime = 0.0f;
                    int myRank = other.GetComponent<NetPlayerInfo>().myRank.Value;
                    ulong objid = other.GetComponent<NetworkObject>().NetworkObjectId;
                    // Give specific item if named, otherwise random item
                    caster.GiveItem(
                        string.IsNullOrEmpty(itemName) ? manager.GetRandomItem(myRank,true,objid) : manager.GetItem(itemName),
                        ammo, false);
                }
            }
        }
    }
}