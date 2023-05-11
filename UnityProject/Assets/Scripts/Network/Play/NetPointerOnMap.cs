using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum PointerType
{
    MY_RED = 0,
    OTHER_RED = 1,
    MY_BLUE = 2,
    OTHER_BLUE = 3
}
public class NetPointerOnMap : MonoBehaviour
{
    Transform[] Pointers;

    private void Awake()
    {
        Pointers = gameObject.GetComponentsInChildren<Transform>();
    }

    public void SetPointer(PointerType _type)
    {
        for(int i=0; i<Pointers.Length; i++)
        {
            Pointers[i].gameObject.SetActive(i == (int)_type ? true : false);
        }
    }



}
