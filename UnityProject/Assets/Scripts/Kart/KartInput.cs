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
        if (!CompareTag("AI"))
        {
            Debug.Log("Not AI");
            Hmove = Input.GetAxis("Horizontal");
            Vmove = Input.GetAxis("Vertical");
            Drift = Input.GetKey(KeyCode.LeftShift);

            if(Input.GetKeyDown(KeyCode.LeftControl))
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
