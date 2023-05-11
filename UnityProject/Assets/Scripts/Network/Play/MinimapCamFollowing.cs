using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamFollowing : MonoBehaviour
{
    public Transform target;
    Vector3 Offset = new Vector3(0, 100, 30);
    
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + Offset;
            transform.rotation = target.rotation;
        }
    }
}
