using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using FMODUnity;

public class RotationSphere : Interactible
{
    Renderer rendR;
    List<ButtonInteractor> currentGrabbers = new();
    [SerializeField] Transform offset, winAngle;
    Color startColor;
    Quaternion lastRot;
    [NonSerialized] public bool correctRot = false;
    bool beaten = false;
    [SerializeField] Lever lever;
    [SerializeField] GameEvent puzzleDone;

    [Space]
    [SerializeField] SoundController sfx;
    [SerializeField] EventReference correctSound;
    [SerializeField] EventReference incorrectSound;

    // Start is called before the first frame update
    void Start()
    {
        rendR = GetComponent<Renderer>();
        lever.leverUpdate += OnLeverUpdate;
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
        currentGrabbers.Add(e.interactor);
        Quaternion pastRot = transform.rotation;
        transform.LookAt(currentGrabbers[0].transform.position, currentGrabbers.Count == 2 ? currentGrabbers[1].transform.position - transform.position : transform.up);
        Quaternion diff = pastRot * Quaternion.Inverse(transform.rotation);
        offset.rotation = diff * offset.rotation;
        e.interactor.buttonUnpress += OnUpress;
    }

    private void OnUpress(object sender, ButtonEventArgs e)
    {
        currentGrabbers.Remove(e.interactor);
        if (currentGrabbers.Count == 1)
        {
            Quaternion pastRot = transform.rotation;
            transform.LookAt(currentGrabbers[0].transform.position, currentGrabbers.Count == 2 ? currentGrabbers[1].transform.position - transform.position : transform.up);
            Quaternion diff = pastRot * Quaternion.Inverse(transform.rotation);
            offset.rotation = diff * offset.rotation;
        }
        e.interactor.buttonUnpress -= OnUpress;
    }

    void Update()
    {
        if (currentGrabbers.Count != 0 && !beaten)
        {
            transform.LookAt(currentGrabbers[0].transform.position, currentGrabbers.Count == 2 ? currentGrabbers[1].transform.position - transform.position : transform.up);
            if (Quaternion.Angle(offset.rotation, winAngle.rotation) <= 7) correctRot = true;
            else correctRot = false;
            lastRot = offset.rotation;
        }
    }

    private void OnLeverUpdate(object sender, LeverEventArgs e)
    {
        if (currentGrabbers.Count != 0) currentGrabbers = new();
        if (correctRot)
        {
            offset.rotation = Quaternion.Lerp(offset.rotation, winAngle.rotation, e.value);
            if (e.value == 1)
            {
                // play sound to indicate correct solution
                sfx.emitter.EventReference = correctSound;
                sfx.emitter.Play();

                beaten = true;
                rendR.material.color = new Color(0, 1, 0, 0.5f);
                puzzleDone.SimpleRaise();
            }
        }
        else if (e.value == 1)
        {
            // play sound to indicate incorrect solution
            sfx.emitter.EventReference = incorrectSound;
            sfx.emitter.Play();
        }
    }
}