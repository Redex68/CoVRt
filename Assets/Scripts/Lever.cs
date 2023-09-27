using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class LeverEventArgs : EventArgs
{
    public float value;
    public LeverEventArgs(float value)
    {
        this.value = value;
    }
}

public class Lever : Interactible
{
    public delegate void LeverEventHandler(object sender, LeverEventArgs e);
    public event LeverEventHandler leverUpdate;

    Renderer rendR;
    ButtonInteractor currentGrabber = new();
    [SerializeField] Transform handleBase, lever, startRot, endRot;
    [SerializeField] float resetTime;
    [SerializeField] RotationSphere sphere;
    bool finished = false;
    Quaternion animationStartPos, lastRot;
    float resetTimer;
    Vector3 offset;
    Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        rendR = GetComponent<Renderer>();
        startColor = rendR.material.color;
    }

    public override void OnEntered(ButtonInteractor interactor)
    {
        //rendR.material.color = Color.cyan;
        interactor.buttonPress += OnPress;
    }

    public override void OnExited(ButtonInteractor interactor)
    {
        //rendR.material.color = startColor;
        interactor.buttonPress -= OnPress;
    }

    private void OnPress(object sender, ButtonEventArgs e)
    {
        currentGrabber = e.interactor;
        offset = transform.position - e.interactor.transform.position;
        e.interactor.buttonUnpress += OnUpress;
    }

    private void OnUpress(object sender, ButtonEventArgs e)
    {
        currentGrabber = null;
        resetTimer = 0;
        animationStartPos = handleBase.rotation;
        e.interactor.buttonUnpress -= OnUpress;
    }

    void Update()
    {
        if (finished) return;
        lastRot = handleBase.rotation;
        if (currentGrabber != null)
        {
            handleBase.LookAt(Vector3.ProjectOnPlane(currentGrabber.transform.position, handleBase.right) + Vector3.Dot(transform.position, handleBase.right) * handleBase.right, Vector3.up);
            if (handleBase.rotation.x < startRot.rotation.x) handleBase.rotation = startRot.rotation;
            else if (handleBase.rotation.x > endRot.rotation.x) handleBase.rotation = endRot.rotation;
            if (handleBase.rotation.x == endRot.rotation.x && sphere.correctRot) { 
                finished = true;
            };
        }
        else if (resetTime != 0 && resetTimer <= resetTime)
        {
            resetTimer += Time.deltaTime;
            handleBase.rotation = Quaternion.Lerp(animationStartPos, startRot.rotation, resetTimer / resetTime);
        }
        if (lastRot != handleBase.rotation)
        {
            leverUpdate?.Invoke(this, new LeverEventArgs((handleBase.rotation.x - startRot.rotation.x) / (endRot.rotation.x - startRot.rotation.x)));
        }
    }
}
