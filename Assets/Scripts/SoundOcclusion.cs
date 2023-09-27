using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundOcclusion : MonoBehaviour
{
    public AudioMixerGroup amg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*float val = -1;
        amg.audioMixer.GetFloat("masterVol", out val);
        Debug.Log(val);*/
        if (Input.GetKey(KeyCode.Alpha1)) Debug.Log(amg.audioMixer.SetFloat("Volume", 10));
        else if (Input.GetKey(KeyCode.Alpha2)) Debug.Log(amg.audioMixer.SetFloat("Volume", 0));
    }
}
