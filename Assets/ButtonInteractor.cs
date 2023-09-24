using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonInteractor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The reference to the action to press Buttons")]
    InputActionReference m_buttonPressAction;

    public event EventHandler buttonPress;

    // Start is called before the first frame update
    void Start()
    {

        var buttonPressAction = m_buttonPressAction != null ? m_buttonPressAction.action : null;
        if (buttonPressAction != null)
        {
            buttonPressAction.performed += OnButtonPress;
        }
    }

    void OnButtonPress(InputAction.CallbackContext context)
    {
        buttonPress?.Invoke(this, new EventArgs());
    }
}