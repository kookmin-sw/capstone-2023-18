using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float m_DestroytTime;
    float m_Time;

    // Start is called before the first frame update
    void Start()
    {
        m_Time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > m_Time + m_DestroytTime)
            Destroy(gameObject);
    }
}
