using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartInput : MonoBehaviour
{
    public float Hmove;
    public float Vmove;
    public bool Drift;
    public bool Item;


    // Update is called once per frame
    void Update()
    {
        
        Hmove = Input.GetAxis("Horizontal");
        Vmove = Input.GetAxis("Vertical");
        Drift = Input.GetKey(KeyCode.LeftShift);
        Item = Input.GetKeyUp(KeyCode.LeftControl);
        
    }

}
