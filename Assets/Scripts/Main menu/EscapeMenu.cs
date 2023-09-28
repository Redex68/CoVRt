using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject Tutorial;

    private bool tutorialPrevActive = false;

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(MainMenu.activeSelf)
            {
                MainMenu.SetActive(false);
                Tutorial.SetActive(tutorialPrevActive);
            }
            else
            {
                MainMenu.SetActive(true);
                tutorialPrevActive = Tutorial.activeSelf;
                Tutorial.SetActive(false);
            }
        }        
    }
}
