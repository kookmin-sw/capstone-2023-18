using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShieldActivate : MonoBehaviour
{
    public float ImpactLife;
    Vector4[] points;
    Material m_material;
    List<Vector4> Hitpoints;
    MeshRenderer m_meshRenderer;
    float time;

    void Start()
    {
        time = Time.time;
        points = new Vector4[30];
        Hitpoints = new List<Vector4>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_material = m_meshRenderer.material;
    }

    void Update()
    {
        //Set material ( based on Shader_IntegratedEffect ) point array
        m_material.SetVectorArray("_Points", points);

        //Find available points 
        Hitpoints = Hitpoints
        .Select(s => new Vector4(s.x, s.y, s.z, s.w + Time.deltaTime / ImpactLife))
        .Where(w => w.w <= 1).ToList();

        //Fill empty point for list circle
        if (Time.time > time + 0.1f)
        {
            time = Time.time;
            AddEmpty();
        }

        //Set array
        Hitpoints.ToArray().CopyTo(points, 0);
    }

    public void AddHitObject(Vector3 position)
    {
        position -= transform.position;
        position = position.normalized/2;
        Hitpoints.Add(new Vector4(position.x, position.y, position.z, 0));
    }

    public void AddEmpty()
    {
        Hitpoints.Add(new Vector4(0, 0, 0, 0));
    }
}
