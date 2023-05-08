using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActive : MonoBehaviour
{
    public GameObject[] m_activeObj;
    public float m_delayTime;
    float m_time;

    private void Start()
    {
        m_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_time + m_delayTime)
            for(int i = 0; i< m_activeObj.Length; i++)
                if(m_activeObj[i] != null)
                    m_activeObj[i].SetActive(true);
    }
}
