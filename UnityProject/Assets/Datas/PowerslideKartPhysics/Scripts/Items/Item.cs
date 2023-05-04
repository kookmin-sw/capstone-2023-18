// Copyright (c) 2022 Justin Couch / JustInvoke
using UnityEngine;
using System.Collections;
using Unity.Netcode;

namespace PowerslideKartPhysics
{
    // Class for different items to be used
    public abstract class Item : NetworkBehaviour
    {
        
        public string itemName = "Item";
        protected ItemCastProperties castProps;
        //protected NetKartController[] allKarts = new NetKartController[0];

        protected virtual void Awake() {
            //allKarts = FindObjectsOfType<NetKartController>();
        }

        // Called upon activation
        public virtual void Activate(ItemCastProperties props, ulong userid, ulong objectid) {
            //props.allKarts = allKarts;
            castProps = props;
            
            
        }

        // Called upon deactivation
        public virtual void Deactivate() {
        }
    }
}