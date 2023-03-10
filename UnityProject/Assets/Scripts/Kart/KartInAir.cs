using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartInAir : MonoBehaviour
{
    KartController controll;
    public Vector3 velocity;
    private void Awake()
    {
        controll = GetComponent<KartController>();
    }

    private void FixedUpdate()
    {
        if (!controll.grounded)
        {
            velocity = controll.rb.velocity;
            velocity.y =  0;
            // Calculate new rotation
            Quaternion targetRotation = Quaternion.LookRotation(velocity, transform.up);
            // Smoothly rotate towards target rotation
            controll.rb.rotation = Quaternion.Slerp(controll.rb.rotation, targetRotation, Time.fixedDeltaTime * 10f);

            
        }

    }
}
