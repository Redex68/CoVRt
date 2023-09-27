using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    [SerializeField] string correctCode;
    [SerializeField] TextMeshPro text;
    [SerializeField] GameEvent doorUnlocked;
    string currentCode = "";
    float resetTime = 0.3f, resetTimer;
    bool failed;

    void Start()
    {

    }

    public void AddValue(string value)
    {
        currentCode += value;
        text.text = "*";
        if (currentCode.Length >= correctCode.Length)
        {
            if (currentCode == correctCode) 
            {  
                Correct();
            }
            else
            {
                Fail();
            }
        }
    }

    void Update()
    {
        if (failed)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer > resetTime) DoReset();
        }
    }

    void Fail()
    {
        failed = true;
        currentCode = "";
        text.text = "No";
    }

    void DoReset()
    {
        resetTimer = 0;
        failed = false;
        currentCode = "";
        text.text = "";
    }

    void Correct()
    {
        text.text = "unlock";
        doorUnlocked.SimpleRaise();
    }
}
