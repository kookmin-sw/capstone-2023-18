using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFactorApplyToMaterial : MonoBehaviour
{
    ParticleSystemRenderer ps;
    float value;
    float m_scaleFactor;
    float m_changedFactor;

    private void Awake()
    {
        ps = this.GetComponent<ParticleSystemRenderer>();
        value = ps.material.GetFloat("_NoiseScale");
        m_scaleFactor = 1;
    }

    void Update()
    {
        m_changedFactor = VariousEffectsScene.m_gaph_scenesizefactor; //Please change this in your actual project

        if(m_scaleFactor != m_changedFactor && m_changedFactor <= 1)
        {
            m_scaleFactor = m_changedFactor;
            if (m_scaleFactor <= 0.5f)
                ps.material.SetFloat("_NoiseScale", value * 0.25f);
            else
                ps.material.SetFloat("_NoiseScale", value * m_scaleFactor);
        }
    }
}
