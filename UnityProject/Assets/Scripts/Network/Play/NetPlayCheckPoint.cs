using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayCheckPoint : MonoBehaviour
{
    public CP[] CP;
    public NetPlayManager npm;
    // Start is called before the first frame update
    void Awake()
    {
        init();
    }

    void init()
    {
        npm = GameObject.Find("@PlayManager").GetComponent<NetPlayManager>();

        CP = gameObject.GetComponentsInChildren<CP>();
        for(int i=0; i<CP.Length; i++)
        {
            CP[i].CheckPointNum = i;
        }

        npm.MaxCP = CP.Length;
    }
}
