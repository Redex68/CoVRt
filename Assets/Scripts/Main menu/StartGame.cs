using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject DefaultInterface;

    public void GameStart()
    {
        DefaultInterface.SetActive(true);
        MainMenu.SetActive(false);
    }

}
