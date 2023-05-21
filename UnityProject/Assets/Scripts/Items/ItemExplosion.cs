using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class ItemExplosion : NetworkBehaviour
{
    [SerializeField]
    private GameObject effect;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Kart") || collision.gameObject.CompareTag("Wall"))
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            onExplosionClientRpc();
        }
        
    }

    [ClientRpc]
    private void onExplosionClientRpc()
    {
        StartCoroutine(onEffect(0.5f));
    }

    IEnumerator onEffect(float time)
    {
        effect.SetActive(true);
        while (time+1f > 0)
        {
            time -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        effect.SetActive(false);
        if(IsServer)gameObject.GetComponent<NetworkObject>().Despawn();
    }
    
     
}
