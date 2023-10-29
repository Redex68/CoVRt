using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DistortionTuner : MonoBehaviour
{
    [SerializeField] private Dial dial;

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

    //Called when user selects a new camera
    public void CameraSwitched()
    {
        RandomizeTargetFrequency();
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomizeTargetFrequency();
        var value = GetComponent<RectTransform>().rect.size;
        Debug.Log(value);
        toScreen.material.SetVector("_Dimensions",value);
    }

    // TODO: subscribe this to whatever event is called when switching camera
    void RandomizeTargetFrequency() {
        target = Random.value;
    }

    void AdjustDistortion() {
        toScreen.material.SetFloat("_Intensity", difference * intensityScale);      
    }
    void AdjustFramerate() {
        framerateLimiter.updatePeriod = Mathf.Lerp(0, periodBound, difference * framerateSensitivity);
    }

    // Update is called once per frame
    void Update()
    {
        value = dial.GetOutput();
        // could be optimized to only be called when there's a change
        difference = Mathf.Abs(target - value);
        AdjustDistortion();
        AdjustFramerate();
    }
}
