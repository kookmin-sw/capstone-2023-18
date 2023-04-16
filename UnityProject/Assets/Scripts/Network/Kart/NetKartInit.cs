using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetKartInit : NetworkBehaviour
{
    //카트가 처음 생성 될 때 Setting
    public GameObject Cam;

    void Start()
    {

        if(IsOwner)
        {
            Cam.SetActive(true);
        }
        else
        {
            Cam.SetActive(false);
        }
        
    }


}
