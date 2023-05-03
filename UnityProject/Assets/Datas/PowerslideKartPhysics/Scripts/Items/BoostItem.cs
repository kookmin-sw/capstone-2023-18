// Copyright (c) 2022 Justin Couch / JustInvoke
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerslideKartPhysics
{
    // Class for boost item
    public class BoostItem : Item
    {
        public float boostAmount = 1.0f;
        public float boostForce = 1.0f;

        public KartController kart;

        
        // Award boost to kart upon activation
        public override void Activate(ItemCastProperties props) {
            base.Activate(props);
            if (props.castKart != null) {
                StartCoroutine(props.castKart.OnBooster(2f));
            }
        }
    }
}