using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;

namespace PowerslideKartPhysics
{
    public class BuffItem : Item
    {
        public float protectionTime = 1f;
        public override void Activate(ItemCastProperties props)
        {
            base.Activate(props);
            if (props.castKart != null)
            {
                StartCoroutine(props.castKart.OnProtected(protectionTime));
            }
            
        }
    }
}

