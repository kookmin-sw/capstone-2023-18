using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMaterialChange : MonoBehaviour
{
    public bool isParticleSystem;
    public Material m_inputMaterial;
    Material m_objectMaterial;
    MeshRenderer m_meshRenderer;
    ParticleSystemRenderer m_particleRenderer;
    public float m_timeToReduce;
    public float m_reduceFactor =0.0f;
    float m_time;
    float m_submitReduceFactor;
    float m_cutOutFactor;
    public float m_upFactor;
    float upFactor;
    bool isupfactor = true;

    void Awake()
    {
        if (isParticleSystem)
        {
            m_particleRenderer = gameObject.GetComponent<ParticleSystemRenderer>();
            m_particleRenderer.material = m_inputMaterial;
            m_objectMaterial = m_particleRenderer.material;
        }
        else
        {
            m_meshRenderer = gameObject.GetComponent<MeshRenderer>();
            m_meshRenderer.material = m_inputMaterial;
            m_objectMaterial = m_meshRenderer.material;
        }
        m_submitReduceFactor = 0.0f;
        m_cutOutFactor = 1.0f;
    }

    void LateUpdate()
    {
        m_time += Time.deltaTime;
        if (m_time > m_timeToReduce)
        {
            m_cutOutFactor -= m_submitReduceFactor;
            m_submitReduceFactor = Mathf.Lerp(m_submitReduceFactor, m_reduceFactor, Time.deltaTime / 50);
        }

        m_cutOutFactor = Mathf.Clamp01(m_cutOutFactor);
        if (m_cutOutFactor <= 0 && m_time > m_timeToReduce)
            Destroy(gameObject);
        m_objectMaterial.SetFloat("_MaskCutOut", m_cutOutFactor);

        if (m_upFactor != 0 && isupfactor != false)
        {
            upFactor += m_upFactor * Time.deltaTime;
            upFactor = Mathf.Clamp01(upFactor);
            m_objectMaterial.SetFloat("_MaskCutOut", upFactor);
            if (upFactor >= 1)
                isupfactor = false;
        }
    }
}
