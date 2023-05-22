using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetwarning : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("disactive", 1);
    }
    
    void disactive()
    {
        gameObject.SetActive(false);
    }
}
