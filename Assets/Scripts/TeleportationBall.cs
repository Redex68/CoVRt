using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TeleportationBall : MonoBehaviour
{
    [NonSerialized] public Transform target;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] float despawnTime;
    float despawnTimer;
    Vector3 lastPos;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        despawnTimer += Time.deltaTime;
        if (despawnTimer >= despawnTime) Destroy(this);
        if (target != null)
        {
            lastPos = transform.position;
            transform.position = target.position;
        }
    }

    public void ThrowBall()
    {
        rb.useGravity = true;
        target = null;
        rb.velocity = (transform.position - lastPos) / Time.deltaTime;
    }

    public Vector3? ActivateBall() 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, GroundLayer))
        {
            return hit.point;
        }
        return null;
    }
}
