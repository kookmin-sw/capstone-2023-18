using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetSkidMakrs : NetworkBehaviour
{
    private TrailRenderer skidMark;
    private ParticleSystem smoke;
    public NetKartController kartController;

    public bool drifting;
    public float driftTime;
    float driftTimeAmount = 0.25f;

    public AudioSource driftSound;
    // 드리프트 키를 누르고 일정 각도 미만으로  떨어지면 fasle
    // 누르고 있는 동안에는 true
    private void Awake()
    {

            smoke = GetComponent<ParticleSystem>();
            skidMark = GetComponent<TrailRenderer>();
            driftSound = GetComponent<AudioSource>();
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
        if (IsOwner)
        {
            checkingDrift();
            Skid();
        }
    }

    void Skid()
    {

        if (kartController.grounded)
        {

            if (drifting)
            {
                skidMark.emitting = true;
                driftSound.mute = false;
                smoke.Play();
            }
            else
            {
                skidMark.emitting = false;
                driftSound.mute = true;
                smoke.Stop();
            }
        }
        else
        {
            skidMark.emitting = false;
            smoke.Stop();
        }
    }

    void checkingDrift()
    {
        Vector3 velocity = kartController.carVelocity;

        if (kartController.input.Drift)
        {
            driftTime = driftTimeAmount;
            drifting = true;
        }
        else if (driftTime > 0)
        {
            if (Mathf.Abs(velocity.x) > kartController.SkidEnable)
            {
                drifting = true;
            }
            else
            {
                driftTime -= Time.fixedDeltaTime;
                drifting = false;
            }
        }
        else if (driftTime <= 0)
        {
            drifting = false;
        }
    }

}
