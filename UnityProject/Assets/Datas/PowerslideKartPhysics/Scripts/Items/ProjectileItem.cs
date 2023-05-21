// Copyright (c) 2022 Justin Couch / JustInvoke

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace PowerslideKartPhysics
{
    // Class for projectile items (this is the casting/spawning class, not the actual spawned items. See SpawnedProjectileItem for spawned item class)
    public class ProjectileItem : Item
    {
        
        public GameObject itemPrefab;
        GameObject spawnedItem;
        public Vector3 spawnOffset;
        
        [ServerRpc(RequireOwnership = false)]
        public override void ActivateServerRpc(ItemCastProperties props, ulong userid, ulong objectid) {
            base.ActivateServerRpc(props,userid,objectid);
            if (itemPrefab != null) {
                // Spawn projectile upon activation

                if (itemName == "Podu")
                {
                    //1등의 정보를 받아와야
                    Debug.Log(spawnOffset);
                    Vector3 no1pos = ItemManager.instance.No1Player.transform.position+ castProps.castRotation * spawnOffset;
                    spawnedItem = Instantiate(itemPrefab, no1pos, castProps.castRotation);
                }
                else spawnedItem = Instantiate(itemPrefab, castProps.castPoint + castProps.castRotation * spawnOffset, castProps.castRotation);
                spawnedItem.GetComponent<NetworkObject>().Spawn();
                SpawnedProjectileItem projectile = spawnedItem.GetComponent<SpawnedProjectileItem>();
                if (projectile != null)
                {
                    projectile.InitializeClientRpc(castProps, userid, objectid);
                }
                
            }
        }
        
     
        // Destroy spawned item upon deactivation
        public override void Deactivate() {
            base.Deactivate();
            if (spawnedItem != null)
            {
                spawnedItem.GetComponent<NetworkObject>().DontDestroyWithOwner = true;
                spawnedItem.GetComponent<NetworkObject>().Despawn();
                Destroy(spawnedItem);
                spawnedItem = null;
            }
        }
    }
}