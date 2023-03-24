using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartInput : MonoBehaviour
{
    public float Hmove;
    public float Vmove;
    public bool Drift;
    public bool Item;
    public bool Return;


    // Update is called once per frame
    void Update()
    {
        if (!CompareTag("AI"))
        {
            Debug.Log("Not AI");
            Hmove = Input.GetAxisRaw("Horizontal");
            Vmove = Input.GetAxisRaw("Vertical");
            Drift = Input.GetKey(KeyCode.LeftShift);
            if(Input.GetKey(KeyCode.R))
            {
                Return = true;
            }

            if(Input.GetKeyDown(KeyCode.LeftControl) && !PlayManager.isReturning)
            {
                Item = true;
            }
            else if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                Item = false;
            }
        }

    }

}
