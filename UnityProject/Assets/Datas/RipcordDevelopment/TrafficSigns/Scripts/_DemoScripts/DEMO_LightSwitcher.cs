using UnityEngine;
using System.Collections;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 						    Traffic Signs 1.0, Copyright © 2017, Ripcord Development
//										   DEMO_LightSwitcher.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script toggles between the sun light and the spot light at the press of a button

namespace Ripcord.Demo
{
    public class DEMO_LightSwitcher : MonoBehaviour
    {
        public GameObject sunLight;
        public GameObject spotLight;

        void Update ()
        {
            if (Input.GetKeyDown(KeyCode.Tab) )
            {
                sunLight.SetActive(!sunLight.activeSelf);
                spotLight.SetActive(!spotLight.activeSelf);
            }
        }
    }
}
