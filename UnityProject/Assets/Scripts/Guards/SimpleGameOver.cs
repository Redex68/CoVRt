using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleGameOver : MonoBehaviour
{
    [SerializeField]
    TMP_Text Text;

    public void OnGameOver() 
    {
        Text.enabled = true;
    }
}
