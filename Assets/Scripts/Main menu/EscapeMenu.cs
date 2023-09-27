using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;
    

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(MainMenu.activeSelf) MainMenu.SetActive(false);
            else MainMenu.SetActive(true);
        }        
    }
}
