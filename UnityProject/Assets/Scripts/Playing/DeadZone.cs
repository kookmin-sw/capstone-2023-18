using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    PlayCheckPoint playcheckpoint;
    void Start()
    {
        playcheckpoint = GameObject.Find("@PlayManager").GetComponent<PlayCheckPoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kart"))
        {
            StartCoroutine(playcheckpoint.ReturnToCP());
        }
    }
}
