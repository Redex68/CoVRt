using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    public EventReference soundEvent;
    [Range(0, 200)]
    public int volume = 100;
    [SerializeField]
    public bool playOnStart = true;

    private EventInstance soundInstance;
    private float origVolume;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        soundInstance = RuntimeManager.CreateInstance(soundEvent);
        soundInstance.getVolume(out origVolume);
        RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);

        if (playOnStart) Play();
    }

    // Update is called once per frame
    void Update()
    {
        // set volume to a percentage of the original one
        // NOTE: subject to change, maybe it's better to use decibels (idk)
        soundInstance.setVolume(origVolume * ((float)volume / 100));   
    }

    // play the sound event (also continues from the paused point, like the pause method)
    public void Play()
    {
        if (paused) Pause();
        else soundInstance.start();
    }

    // pause the sound event (play again if already paused)
    public void Pause()
    {
        paused = !paused;
        soundInstance.setPaused(paused);
    }
}
