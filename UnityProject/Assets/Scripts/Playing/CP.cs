using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CP : MonoBehaviour
{
    public int CheckPointNum;
    public CP[] NextCheckPoint;
    PlayCheckPoint playcheckpoint;
    public bool CorrectNext;

    private void Start()
    {
        playcheckpoint = GameObject.Find("@PlayManager").GetComponent<PlayCheckPoint>();
    }

    public Transform GetReturnPoint()
    {
        return gameObject.transform;
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Kart"))
        {
            CorrectNext = false;
            for (int i = 0; i < PlayManager.nextCP.Length; i++)
            {
                if (PlayManager.nextCP[i].CheckPointNum == CheckPointNum)
                {
                    CorrectNext = true;
                }
            }
            if (CorrectNext || PlayManager.CP == CheckPointNum)
            {
                //올바르게 주행 한 경우
                PlayManager.CP = CheckPointNum;
                PlayManager.nextCP = NextCheckPoint;
            }
            else
            {
                //역주행 등 올바르지 않은 주행인 경우
                StartCoroutine(playcheckpoint.ReturnToCP());
            }
        }
    }
}
