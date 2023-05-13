using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemBox : NetworkBehaviour
{
    private void Start()
    {
        gameObject.layer = 0; //Default Layer;
        gameObject.GetComponent<NetworkObject>().DestroyWithScene = true;
    }

    public override void OnNetworkSpawn()
    {
        gameObject.GetComponent<NetworkObject>().DestroyWithScene = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kart") && gameObject.layer != 30)
        {
            StartCoroutine(RemakeBox(other.transform.GetComponent<NetKartController>()));
        }
    }

    int SelectItem()
    {
        //�������� ȹ���� ������ ��Ȳ�� ���߾� �������� ������.
        //����� �ν��͹ۿ� �����Ƿ� �ν��͸�.
        //return (int)ITEMS.BOOST;
        return 0;
    }

    IEnumerator RemakeBox(NetKartController user)
    {
        //������ ȹ�� ��, �ڽ� disable
        //n�� �� �����
        user.npi.Item.Value = SelectItem();
        gameObject.layer = 30;
        yield return new WaitForSeconds(2f);
        gameObject.layer = 0;
    }
}
