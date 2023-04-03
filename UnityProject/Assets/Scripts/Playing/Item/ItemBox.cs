using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private void Start()
    {
        gameObject.layer = 0; //Default Layer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kart") && gameObject.layer != 30)
        {
            StartCoroutine(RemakeBox(other.transform.GetComponent<NetKartController>()));
        }
    }

    ITEMS SelectItem()
    {
        //아이템을 획득한 유저의 상황에 맞추어 아이템을 설정함.
        //현재는 부스터밖에 없으므로 부스터만.
        return ITEMS.BOOST;
    }

    IEnumerator RemakeBox(NetKartController user)
    {
        //아이템 획득 시, 박스 disable
        //n초 후 재생성
        user.hasItem = SelectItem();
        gameObject.layer = 30;
        yield return new WaitForSeconds(2f);
        gameObject.layer = 0;
    }
}
