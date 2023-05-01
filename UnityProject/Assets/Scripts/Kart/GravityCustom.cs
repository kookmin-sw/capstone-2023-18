using UnityEngine;

public class GravityCustom : MonoBehaviour
{
    private Rigidbody rb;

    public float gravity = -30f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * (gravity) * rb.mass);
    }
}
