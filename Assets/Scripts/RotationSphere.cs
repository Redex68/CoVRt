using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class RotationSphere : Interactible
{
    Renderer rendR;
    List<ButtonInteractor> currentGrabbers = new();
    [SerializeField] Transform offset;
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
        currentGrabbers.Add(e.interactor);
        quaternion pastRot = transform.rotation;
        transform.LookAt(currentGrabbers[0].transform.position, currentGrabbers.Count == 2 ? currentGrabbers[1].transform.position - transform.position : transform.up);
        quaternion diff = pastRot * Quaternion.Inverse(transform.rotation);
        offset.rotation = diff * offset.rotation;
        e.interactor.buttonUnpress += OnUpress;
    }

    private void OnUpress(object sender, ButtonEventArgs e)
    {
        currentGrabbers.Remove(e.interactor);
        if (currentGrabbers.Count == 1)
        {
            quaternion pastRot = transform.rotation;
            transform.LookAt(currentGrabbers[0].transform.position, currentGrabbers.Count == 2 ? currentGrabbers[1].transform.position - transform.position : transform.up);
            quaternion diff = pastRot * Quaternion.Inverse(transform.rotation);
            offset.rotation = diff * offset.rotation;
        }
        e.interactor.buttonUnpress -= OnUpress;
    }

    void Update()
    {
        if (currentGrabbers.Count != 0)
        {
            transform.LookAt(currentGrabbers[0].transform.position, currentGrabbers.Count == 2 ? currentGrabbers[1].transform.position - transform.position : transform.up); 
        }    
    }
}