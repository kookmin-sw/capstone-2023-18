using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerOnMap : MonoBehaviour
{
    public Material m;

    public void Start()
    {
        Material myColor = GetComponent<MeshRenderer>().material;

        myColor = m;
    }
}
