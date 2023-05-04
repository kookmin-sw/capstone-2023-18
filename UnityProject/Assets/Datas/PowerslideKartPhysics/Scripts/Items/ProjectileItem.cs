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
        
        public override void Activate(ItemCastProperties props, ulong userid, ulong objectid) {
            base.Activate(props,userid,objectid);
            Debug.Log("userid : " + userid + " // " + ItemManager.instance.allKarts.Length);
            if (itemPrefab != null) {
                // Spawn projectile upon activation
                spawnedItem = Instantiate(itemPrefab, castProps.castPoint + castProps.castRotation * spawnOffset, castProps.castRotation);
                spawnedItem.GetComponent<NetworkObject>().Spawn();
                SpawnedProjectileItem projectile = spawnedItem.GetComponent<SpawnedProjectileItem>();
                if (projectile != null)
                {
                    projectile.Initialize(castProps, userid);
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