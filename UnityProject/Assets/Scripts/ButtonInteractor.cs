using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class ButtonEventArgs : EventArgs
{
    public ButtonInteractor interactor;
    public ButtonEventArgs(ButtonInteractor interactor)
    {
        this.interactor = interactor;
    }
}

public class ButtonInteractor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The reference to the action to press Buttons")]
    InputActionReference m_buttonPressAction;

    public delegate void ButtonEventHandler(object sender, ButtonEventArgs e);
    public event ButtonEventHandler buttonPress;
    public event ButtonEventHandler buttonUnpress;

    // Start is called before the first frame update
    void Start()
    {

        var buttonPressAction = m_buttonPressAction != null ? m_buttonPressAction.action : null;
        if (buttonPressAction != null)
        {
            buttonPressAction.performed += OnButtonPress;
            buttonPressAction.canceled += OnButtonUnpress;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Button"))
        {
            Interactible obj = other.GetComponent<Interactible>();
            if (obj != null)
            {
                obj.OnEntered(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Button"))
        {
            Interactible obj = other.GetComponent<Interactible>();
            if (obj != null)
            {
                obj.OnExited(this);
            }
        }
    }

    void OnButtonPress(InputAction.CallbackContext context)
    {
        buttonPress?.Invoke(this, new ButtonEventArgs(this));
    }
    void OnButtonUnpress(InputAction.CallbackContext context)
    {
        buttonUnpress?.Invoke(this, new ButtonEventArgs(this));
    }
}