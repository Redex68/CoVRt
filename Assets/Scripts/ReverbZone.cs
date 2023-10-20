using UnityEngine;
using FMODUnity;

public class ReverbZone : MonoBehaviour
{
    // reverb value to assign to a sound source in the zone
    public int reverbValue;
    public bool renderBoundaries = true;
    private bool listenerInZone = false;

    private void Update()
    {
        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer rend in renderers) rend.enabled = renderBoundaries;
    }

    private void OnTriggerEnter(Collider other)
    {
        SoundController soundSource = other.transform.GetComponentInChildren<SoundController>();
        if (soundSource && soundSource.permitReverbZones && listenerInZone) soundSource.reverb = reverbValue;

        StudioListener listener = null;
        if (listener = other.transform.GetComponentInChildren<StudioListener>()) listenerInZone = true;
    }

    private void OnTriggerStay(Collider other)
    {
        SoundController soundSource = null;
        if (soundSource = other.transform.GetComponentInChildren<SoundController>())
        {
            if (listenerInZone && soundSource.permitReverbZones) soundSource.reverb = reverbValue;
            else soundSource.StartReverbDecay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SoundController soundSource = other.transform.GetComponentInChildren<SoundController>();
        if (soundSource && soundSource.permitReverbZones) soundSource.StartReverbDecay();

        StudioListener listener = null;
        if (listener = other.transform.GetComponentInChildren<StudioListener>()) listenerInZone = false;
    }
}
