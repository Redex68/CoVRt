using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject DefaultInterface;
    [SerializeField]
    EscapeMenu escapeMenu;

    public void GameStart()
    {
        DefaultInterface.SetActive(true);
        MainMenu.SetActive(false);
        escapeMenu.enabled = true;
        GetComponent<Button>().interactable = false;
    }

}
