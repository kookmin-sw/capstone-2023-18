// Copyright (c) 2022 Justin Couch / JustInvoke
using UnityEngine;
using System.Collections;

namespace PowerslideKartPhysics
{
    // Class for different items to be used
    public abstract class Item : MonoBehaviour
    {
        public string itemName = "Item";
        protected ItemCastProperties castProps;
        protected KartController[] allKarts = new KartController[0];

        protected virtual void Awake() {
            allKarts = FindObjectsOfType<KartController>();
            Debug.Log(allKarts.Length);
        }

        // Called upon activation
        public virtual void Activate(ItemCastProperties props) {
            props.allKarts = allKarts;
            castProps = props;
        }

        // Called upon deactivation
        public virtual void Deactivate() {
        }
    }
}