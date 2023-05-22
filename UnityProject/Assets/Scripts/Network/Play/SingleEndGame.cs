using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEndGame : MonoBehaviour
{
    public NetPlayManager npm;
    public bool isEnd;


    public GameObject LoseUI;
    public GameObject WinUI;
    private void Start()
    {
        isEnd = false;
        LoseUI.SetActive(false);
        WinUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnd == false)
        {
            Debug.Log(other.name);

            if (other.CompareTag("AI"))
            {
                Debug.Log("yes");
                isEnd = true;
                LoseUI.SetActive(true);
            }

            if (other.CompareTag("Kart"))
            {
                isEnd = true;
                WinUI.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isEnd == false)
        {
            Debug.Log(other.name);

            if (other.CompareTag("AI"))
            {
                Debug.Log("yes");
                isEnd = true;
                LoseUI.SetActive(true);
            }

            if (other.CompareTag("Kart"))
            {
                isEnd = true;
                WinUI.SetActive(true);
            }
        }
    }
}
