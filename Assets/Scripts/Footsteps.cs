using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Footsteps : MonoBehaviour
{
    public StudioEventEmitter emitter;
    private Vector3 oldPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.position - oldPos;

        if (!emitter.IsPlaying() && move != Vector3.zero) emitter.Play();
        else if (move == Vector3.zero) emitter.Stop();

        oldPos = transform.position;
    }
}
