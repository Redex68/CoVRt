
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound: MonoBehaviour
{
    [SerializeField]
    AudioClip clip;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float volume = 1.0f;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClip()
    {
        source.PlayOneShot(clip, volume);
    }
}