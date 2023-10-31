using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(SoundController))]
public class Ambience : MonoBehaviour
{
    private SoundController sound;
    private int origVolume;
    private StudioListener listener;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<SoundController>();
        origVolume = sound.volume;
        listener = FindObjectOfType<StudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        // project positions onto XZ-plane
        Vector3 listenerPos = listener.transform.position;
        listenerPos.y = 0;
        Vector3 soundPos = transform.position;
        soundPos.y = 0;

        // check if listener is close enough for ambience
        sound.emitter.EventInstance.getMinMaxDistance(out _, out float maxDist);
        if (Vector3.Distance(listenerPos, soundPos) <= maxDist) sound.volume = origVolume;
        else sound.volume = 0;
    }
}
