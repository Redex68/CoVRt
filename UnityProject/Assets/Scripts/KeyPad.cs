using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class KeyPad : MonoBehaviour
{
    [SerializeField] string correctCode;
    [SerializeField] TextMeshPro text;
    [SerializeField] GameEvent doorUnlocked;
    string currentCode = "";
    float resetTime = 0.3f, resetTimer;
    bool reset;

    void Start()
    {

    }

    public void AddValue(string value)
    {
        currentCode += value;
        text.text += "*";
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

        RuntimeManager.PlayOneShot("event:/beepboop", transform.position);
    }

    void Update()
    {
        if (reset)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer > resetTime) DoReset();
        }
    }

    void Fail()
    {
        reset = true;
        currentCode = "";
        text.text = "Fail";

        RuntimeManager.PlayOneShot("event:/accessdenied", transform.position);
    }

    void DoReset()
    {
        resetTimer = 0;
        reset = false;
        currentCode = "";
        text.text = "";
    }

    void Correct()
    {
        reset = true;
        text.text = "Unlocked";
        doorUnlocked.SimpleRaise();

        RuntimeManager.PlayOneShot("event:/accessgranted", transform.position);
    }
}
