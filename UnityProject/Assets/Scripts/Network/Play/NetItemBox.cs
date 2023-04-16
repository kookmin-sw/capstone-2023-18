using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetItemBox : NetworkBehaviour
{
    Vector3 UP = new Vector3(0, 0.005f, 0);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            StartCoroutine(Updown());
        }
    }

    void FixedUpdate()
    {
        if (IsServer)
        {
            transform.Rotate(Vector3.up * 20 * Time.deltaTime);
        }
    }

    IEnumerator Updown()
    {
        while (true)
        {
            for (int i = 0; i < 250; i++)
            {
                transform.position += UP;
                yield return new WaitForFixedUpdate();
            }
            for (int i = 0; i < 250; i++)
            {
                transform.position -= UP;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kart") && IsServer)
        {
            ulong _client = other.gameObject.GetComponent<NetworkObject>().OwnerClientId;
            gameObject.SetActive(false);
            gameObject.GetComponent<NetworkObject>().Despawn(false);

            //아이템 부여
            NetPlayerInfo info = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(_client).GetComponent<NetPlayerInfo>();
            info.Item.Value = GetItemKind();
        }

    }

    int GetItemKind()
    {
        //유저 등수나 역할에 따라 아이템 리턴
        //이 부분은 추후 개선 예정이며, 현재는 부스터만 리턴
        return 1;
    }

}
