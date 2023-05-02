using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KartMoveMent : NetworkBehaviour
{
    Transform KartTransform;
    NetworkVariable<Vector3> NetworkPosition = new(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);
    NetworkVariable<Quaternion> NetworkRotation = new(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);

    void Start()
    {
        KartTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {
            NetworkPosition.Value = KartTransform.transform.position;
            NetworkRotation.Value = KartTransform.transform.rotation;
        }
        else
        {
            KartTransform.position = NetworkPosition.Value;
            KartTransform.rotation = NetworkRotation.Value;
        }
    }
}
