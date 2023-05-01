using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private GameObject player;

    private Rigidbody m_rigid;

    private float speed = 20f;
    private float current_speed = 50f;
    private LayerMask _layerMask;


    private void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        current_speed += speed * Time.deltaTime;
        transform.LookAt(target.transform);
        transform.position += transform.forward * current_speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {

            //makeSpin(collision);
            Destroy(gameObject);
        }
    }

    
}
