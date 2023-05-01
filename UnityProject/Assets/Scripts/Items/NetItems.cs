using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpinAxis { Yaw, Pitch, Roll }

public class NetItems : MonoBehaviour
{
    public bool spinningOut = false;
    Vector3 spinForward = Vector3.forward;
    Vector3 spinUp = Vector3.up;
    Vector3 spinOffset = Vector3.zero;

    Rigidbody rb;

    public float spinDecel = 1.0f;
    public float spinRate = 10f;
    public float spinHeight = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Spining Out

    void Spining()
    {
        // Visual rotation while spinning out
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(spinForward, spinUp), 20f * Time.fixedDeltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero + spinOffset, 20f * Time.fixedDeltaTime);
        rb.AddForce(new Vector3(-rb.velocity.x, 0.0f, -rb.velocity.z) * spinDecel, ForceMode.Acceleration); // Slow down while spinning out
    }


    // Spin cycle that calculates the current spin angle
    IEnumerator SpinCycle(SpinAxis spinType, float spinAmount)
    {
        // Spin start
        spinningOut = true;
        float spinDir = Mathf.Sign(0.5f - Random.value);
        float curSpin = 0.0f;
        float maxSpin = spinAmount * Mathf.PI * 2.0f;

        // Actual spin cycle
        while (Mathf.Abs(curSpin) < maxSpin)
        {
            curSpin += spinDir * spinRate * Mathf.Clamp((maxSpin - Mathf.Abs(curSpin)), 0.1f, 1.0f) * Time.fixedDeltaTime;
            switch (spinType)
            {
                case SpinAxis.Yaw:
                    spinForward = new Vector3(Mathf.Sin(curSpin), Mathf.Sin(curSpin * 2.0f) * 0.1f, Mathf.Cos(curSpin));
                    spinUp = Vector3.up;
                    break;
                case SpinAxis.Roll:
                    spinUp = new Vector3(Mathf.Sin(curSpin), Mathf.Cos(curSpin), 0.0f);
                    break;
                case SpinAxis.Pitch:
                    spinForward = new Vector3(0.0f, Mathf.Sin(curSpin), Mathf.Cos(curSpin));
                    spinUp = new Vector3(0.0f, Mathf.Cos(curSpin), -Mathf.Sin(curSpin));
                    break;
            }

            if (spinType != SpinAxis.Yaw)
            {
                spinOffset = Vector3.up * spinHeight * Mathf.Sin((Mathf.Abs(curSpin) / Mathf.Max(maxSpin, 0.001f)) * Mathf.PI);
            }
            yield return new WaitForFixedUpdate();
        }

        // Spin end
        spinningOut = false;
        spinForward = Vector3.forward;
        spinOffset = Vector3.zero;
        spinUp = Vector3.up;
        //boostPadUsed = false;
    }
}
