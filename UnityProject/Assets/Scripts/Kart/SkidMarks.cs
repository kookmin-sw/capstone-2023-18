using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarks : MonoBehaviour
{
    private TrailRenderer skidMark;
    private ParticleSystem smoke;
    public KartController kartController;
    private void Awake()
    {
        smoke = GetComponent<ParticleSystem>();
        skidMark = GetComponent<TrailRenderer>();
        skidMark.emitting = false;
        transform.localPosition = new Vector3(0, -transform.parent.parent.GetComponent<SphereCollider>().radius + 0.03f, 0);
        if (kartController != null)
        {
            skidMark.startWidth = kartController.skidWidth;
        }
    }

    private void OnEnable()
    {
        skidMark.enabled = true;
    }
    private void OnDisable()
    {
        skidMark.enabled = false;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Skid();
    }

    void Skid()
    {
        Vector3 velocity = kartController.carVelocity;


        if (kartController.grounded)
        {

            if (Mathf.Abs(velocity.x) > kartController.SkidEnable)
            {
                skidMark.emitting = true;
                smoke.Play();
            }
            else
            {
                skidMark.emitting = false;
                smoke.Stop();
            }
        }
        else
        {
            skidMark.emitting = false;
            smoke.Stop();
        }
    }
}
