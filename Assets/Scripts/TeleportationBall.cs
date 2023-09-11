using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleportationBall : MonoBehaviour
    {
        public Transform target;
        Vector3 lastPos;
        Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (target != null)
            {
                if (!rb.useGravity)
                {
                    lastPos = transform.position;
                    transform.position = target.position;
                }
                else
                {
                    target = null;
                    rb.velocity = transform.position - lastPos / Time.deltaTime;
                }
            }
        }
    }
