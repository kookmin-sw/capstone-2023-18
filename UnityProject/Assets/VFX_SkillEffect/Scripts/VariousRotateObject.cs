using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousRotateObject : MonoBehaviour {

    public Vector3 RotateOffset;
    Vector3 RotateMulti;
    public float m_delay;
    float m_Time;

    void Awake()
    {
        m_Time = Time.time;
    }

	// Update is called once per frame
	void Update ()
    {
        if (Time.time < m_Time + m_delay)
            return;
        RotateMulti = Vector3.Lerp(RotateMulti,RotateOffset,Time.deltaTime);

        transform.rotation *= Quaternion.Euler(RotateMulti);		
	}
}
