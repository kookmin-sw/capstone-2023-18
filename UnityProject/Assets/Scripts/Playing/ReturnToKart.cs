using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToKart : MonoBehaviour
{
    KartInput input;
    PlayCheckPoint playcheckpoint;

    void Start()
    {
        input = transform.GetComponent<KartInput>();
        playcheckpoint = GameObject.Find("@PlayManager").GetComponent<PlayCheckPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if(input.Return)
        {
            input.Return = false;
            StartCoroutine(playcheckpoint.ReturnToCP());
        }
    }
}
