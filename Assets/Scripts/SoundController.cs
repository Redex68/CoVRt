using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// Class used together with a StudioEventEmitter, to manipulate audio more easily
public class SoundController : MonoBehaviour
{
    public StudioEventEmitter emitter;

    [Space]
    [Range(0, 500)]
    [Tooltip("Volume multiplier (%)")]
    public int volume = 100; 
    [Range(0, 500)]
    [Tooltip("Pitch multiplier (%)")]
    public int pitch = 1;
    [Range(0, 100)]
    [Tooltip("Reverb multiplier (%)")]
    public int reverb = 0;

    [Space]
    public bool useOcclusion = false;
    [Range(0, 10)]
    public float occlusionSpread = 1;
    public LayerMask occlusionLayers;

    // occlusion stuff
    private StudioListener listener;
    private int lineCastsObstructed;
    private int totalLineCasts;

    // Start is called before the first frame update
    void Start()
    {
        // find the audio listener in the scene
        listener = FindObjectOfType<StudioListener>();
        totalLineCasts = 3;
    }

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

        if (useOcclusion) Occlusion();
    }

    [ContextMenu("Play")]
    private void Play()
    {
        emitter.EventInstance.getPaused(out bool paused);
        if (paused) Pause();
        else emitter.Play();
    }

    [ContextMenu("Pause")]
    private void Pause()
    {
        emitter.EventInstance.getPaused(out bool paused);
        emitter.EventInstance.setPaused(!paused);
    }

    private void Occlusion()
    {
        // NOTE: only occlude if sound can be heard (is close enough)
        // check out isVirtual property of the event instance

        lineCastsObstructed = 0;

        OcclusionLineCast(listener.transform.position, transform.position);
        OcclusionLineCast(CalculatePoint(listener.transform.position, transform.position, occlusionSpread, true), transform.position);
        OcclusionLineCast(CalculatePoint(listener.transform.position, transform.position, occlusionSpread, false), transform.position);

        emitter.SetParameter("Occlusion", (float)lineCastsObstructed / totalLineCasts);
        emitter.EventInstance.setParameterByName("Occlusion", (float)lineCastsObstructed / totalLineCasts);
    }

    // Not mine; this is by ScottGamesSounds https://scottgamesounds.com/wp-content/uploads/2020/01/C.FirstPersonOcclusion.txt
    private Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool posOrneg)
    {
        float x;
        float z;
        float n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
        float mn = (m / n);
        if (posOrneg)
        {
            x = a.x + (mn * (a.z - b.z));
            z = a.z - (mn * (a.x - b.x));
        }
        else
        {
            x = a.x - (mn * (a.z - b.z));
            z = a.z + (mn * (a.x - b.x));
        }
        return new Vector3(x, a.y, z);
    }

    private void OcclusionLineCast(Vector3 listener, Vector3 sound)
    {
        RaycastHit hit;
        Physics.Linecast(listener, sound, out hit, occlusionLayers);

        Color color;
        if (hit.collider)
        {
            lineCastsObstructed++;
            color = Color.red;
        }
        else color = Color.green;

        Debug.DrawLine(listener, sound, color);
    }
}
