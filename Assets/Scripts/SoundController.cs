using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// Class used together with a StudioEventEmitter, to manipulate audio more easily
public class SoundController : MonoBehaviour
{
    public StudioEventEmitter emitter;
    [Range(0, 500)]
    [Tooltip("Volume multiplier (%)")]
    public int volume = 100; 
    [Range(0, 500)]
    [Tooltip("Pitch multiplier (%)")]
    public int pitch = 1;
    [Range(0, 100)]
    [Tooltip("Reverb multiplier (%)")]
    public int reverb = 0;

    // Update is called once per frame
    void Update()
    {
        // set volume and pitch to a percentage of the original values
        emitter.EventInstance.setVolume((float)volume / 100);
        emitter.EventInstance.setPitch((float)pitch / 100);

        emitter.EventInstance.setReverbLevel(0, (float)reverb / 100);
        emitter.EventInstance.setReverbLevel(1, (float)reverb / 100);
        emitter.EventInstance.setReverbLevel(2, (float)reverb / 100);
        emitter.EventInstance.setReverbLevel(3, (float)reverb / 100);
    }
}
