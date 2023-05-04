using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;
using Unity.Netcode;
public class SpawnedProjectileItemDir : NetworkBehaviour
{
    // Start is called before the first frame update
    private Transform tr;
    private Vector3 dir;
    private SpawnedProjectileItem item;
    void Start()
    {
        tr = GetComponent<Transform>();
        item = GetComponentInParent<SpawnedProjectileItem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tr.forward = item.moveDir;
    }
}
