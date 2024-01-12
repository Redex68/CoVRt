using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : Interactible
{
    [SerializeField] Transform startPos, pressedPos;
    [SerializeField] string buttonValue;
    [SerializeField] KeyPad keyPad;
    Renderer rendR;

    Color startColor;

    float[] animationTimes = new float[3] { 0.1f, 0.02f, 0.1f };
    Vector3[] animationPositions;
    float animationTimer = 0;
    int animationIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        animationPositions = new Vector3[4] {startPos.position, pressedPos.position, pressedPos.position, startPos.position };
        rendR = GetComponent<Renderer>();
        startColor = rendR.material.color;
    }

    public override void OnEntered(ButtonInteractor interactor)
    {
        rendR.material.color = Color.cyan;
        interactor.buttonPress += OnPress;
    }

    public override void OnExited(ButtonInteractor interactor)
    {
        rendR.material.color = startColor;
        interactor.buttonPress -= OnPress;
    }

    private void OnPress(object sender, ButtonEventArgs e)
    {
        if (animationIndex == -1)
        {
            animationIndex = 0;
            keyPad.AddValue(buttonValue);
        }
    }

    void Update()
    {
        if (animationIndex != -1)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationTimes[animationIndex])
            {
                animationIndex++;
                animationTimer = 0;
                transform.position = animationPositions[animationIndex];
                if (animationIndex == animationTimes.Length)
                {
                    animationIndex = -1;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(animationPositions[animationIndex], animationPositions[animationIndex+1], animationTimer / animationTimes[animationIndex]);
            }
        }
    }
}