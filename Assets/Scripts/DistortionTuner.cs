using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistortionTuner : MonoBehaviour
{
    [SerializeField]
    private CustomRenderTexture toScreen;

    [SerializeField]
    private CustomRenderTexture framerateLimiter;
    [SerializeField]
    private float intensityScale = 100;
    [SerializeField, Range(0,1)]
    private float periodBound = 0.5f;
    [SerializeField]
    private float framerateSensitivity = 10;


    [SerializeField, Range(0,1)]
    private float value = 0;
    private float target;
    private float difference = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        RandomizeTargetFrequency();
    }

    // TODO: subscribe this to whatever event is called when switching camera
    void RandomizeTargetFrequency() {
        target = Random.value;
    }

    void AdjustDistortion() {
        toScreen.material.SetFloat("_Intensity", difference * intensityScale);      
    }
    void AdjustFramerate() {
        framerateLimiter.updatePeriod = Mathf.Lerp(0, periodBound, difference * framerateSensitivity) + 0.1f;
    }


    // Update is called once per frame
    void Update()
    {
        // could be optimized to only be called when there's a change
        difference = Mathf.Abs(target - value);
        AdjustDistortion();
        AdjustFramerate();
    }
}
