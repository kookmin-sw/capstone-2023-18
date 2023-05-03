using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ObjectsMakeBase : MonoBehaviour
{
    public GameObject[] m_makeObjs;

    public float GetRandomValue(float value)
    {
        return Random.Range(-value, value);
    }

    public float GetRandomValue2(float value)
    {
        return Random.Range(0, value);
    }

    public Vector3 GetRandomVector(Vector3 value)
    {
        Vector3 result;
        result.x = GetRandomValue(value.x);
        result.y = GetRandomValue(value.y);
        result.z = GetRandomValue(value.z);
        return result;
    }

    public Vector3 GetRandomVector2(Vector3 value)
    {
        Vector3 result;
        result.x = GetRandomValue2(value.x);
        result.y = GetRandomValue2(value.y);
        result.z = GetRandomValue2(value.z);
        return result;
    }
}
