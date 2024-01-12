using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactible : MonoBehaviour
{
    public abstract void OnEntered(ButtonInteractor interactor);
    public abstract void OnExited(ButtonInteractor interactor);
}
