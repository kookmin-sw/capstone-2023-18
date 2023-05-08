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
            gameObject.GetComponent<NetworkObject>().DestroyWithScene = true;
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

            //������ �ο�
            NetPlayerInfo info = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(_client).GetComponent<NetPlayerInfo>();
            info.Item.Value = GetItemKind();
        }

    }

    int GetItemKind()
    {
        //���� ����� ���ҿ� ���� ������ ����
        //�� �κ��� ���� ���� �����̸�, ����� �ν��͸� ����
        return 1;
    }

}
