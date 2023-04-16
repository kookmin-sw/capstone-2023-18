using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetItemSpawn : NetworkBehaviour
{
    private int numOfitems;
    [SerializeField] Transform[] SpawnPositions;

    [SerializeField] GameObject PrefabOb;
    private GameObject[] m_PrefabOb;
    private NetworkObject[] m_SpawnedNetworkOb;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        numOfitems = SpawnPositions.Length;
        //Object Pooling
        m_PrefabOb = new GameObject[numOfitems];
        m_SpawnedNetworkOb = new NetworkObject[numOfitems];
        for (int i = 0; i < numOfitems; i++)
        {
            m_PrefabOb[i] = Instantiate(PrefabOb);
            m_PrefabOb[i].TryGetComponent<NetworkObject>(out m_SpawnedNetworkOb[i]);
            m_PrefabOb[i].SetActive(false);
        }
        if (IsServer)
        {
            StartCoroutine(InfinityItems());
        }
    }


    public IEnumerator InfinityItems()
    {
        if (IsServer)
        {
            while (true)
            {
                for (int i = 0; i < numOfitems; i++)
                {
                    if (m_PrefabOb[i] != null && m_SpawnedNetworkOb[i] != null && !m_SpawnedNetworkOb[i].IsSpawned)
                    {
                        StartCoroutine(SpawnItem(i));
                    }
                }
                yield return new WaitForSecondsRealtime(20.0f);
            }
        }
    }
    public IEnumerator SpawnItem(int idx)
    {
        Debug.Log("Spawn ITEM!");
        yield return new WaitForSecondsRealtime(1.0f);
        m_PrefabOb[idx].SetActive(true);
        m_SpawnedNetworkOb[idx].Spawn();
        m_PrefabOb[idx].transform.position = SpawnPositions[idx].position;
    }
}
