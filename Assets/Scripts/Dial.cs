using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dial : MonoBehaviour
{
    Transform handle;
    Image fill;
    Image meter;

    public bool UIInteractionEnabled = true;
    
    [SerializeField] int dialIndex;
    // how much can the dial actually turn?
    [SerializeField, Range(180, 360)] public float degrees = 360;

    [SerializeField, Range(360, 0)] public float angle = 180;
    [SerializeField] float threshold = 0.5f; 

    // flag for whether we've "grabbed" the value after changing camera etc
    [SerializeField] public bool grabbed = false;
    [SerializeField, Range(360, 0)] public float outputAngle = 180;


    float offset = 0;

    void Start(){
        handle = transform.Find("Knob");
        meter = transform.Find("Meter").GetComponent<Image>();
        fill = transform.Find("Meter/Fill").GetComponent<Image>();
        SetupOffset();
    }
    public void OnHandleDrag() {
        if(UIInteractionEnabled)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2 direction  = mousePos - handle.position;
            float newAngle = Vector2.SignedAngle(Vector2.down, direction);
            newAngle = newAngle <= 0 ? 360 + newAngle : newAngle;
            if (newAngle >= 360 - offset || newAngle <= offset) return;
            angle = newAngle;
        }
    }
    void TryGrab() {
        // is there a better way to do this?
        if (Mathf.Abs(angle - outputAngle) < threshold) { // will be problematic if we go from 360 to 0
            grabbed = true;
        }
    }
    
    public void Release() {
        grabbed = false;
    }
    
    /// <summary>
    /// Set what the big pointer points to in the range 0-1
    /// </summary>
    /// <param name="input"> number between 0 and 1 to set input to </param>
    public void SetInput(float input) {
        angle = (1 - input) * degrees + offset;
    }

    /// <summary>
    /// Get what the big pointer is pointing at in range 0-1
    /// </summary>
    /// <returns> float in range 0-1 describing the input signal</returns>
    public float GetInput() {
        return 1 - (angle - offset) / degrees;
    }


    // TODO: Subscribe this to whatever event happens when changing cameras. Give it a value that corresponds to how much that camera's pose is rotated from its default.
    /// <summary>
    /// Set what the indicator meter is showing in range 0-1.
    /// Releases the "grab" so that it needs to be grabbed again before further inputs have any effect.
    /// </summary>
    /// <param name="value">number between 0 and 1 to point indicator at</param>
    public void SetOutput(float value) {
        Release(); 
        outputAngle = (1 - value) * degrees + offset;
    }

    /// <summary>
    /// Get what the indicator is pointing at in range 0-1
    /// </summary>
    /// <returns>float in range 0-1 describing the output signal</returns>
    public float GetOutput() {
        return 1 - (outputAngle - offset) / degrees;
    }

    void SetFill(){
        fill.fillAmount = 1 - (outputAngle + offset) / 360;
    }

    void SetRotation() {
        Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.forward);
        handle.rotation = rotate;
    }

    void SetupOffset() {
        offset = (360 - degrees) / 2;
        transform.localRotation = Quaternion.AngleAxis(-offset, Vector3.forward);

        meter.fillAmount = degrees / 360.0f - 0.001f;
    }

    void Update() {
        SetupOffset();
        if(!UIInteractionEnabled)
        {
            angle = ControlPanel.Instance.GetDialPositions()[dialIndex] * degrees + (360.0f - degrees) / 2.0f;
        }
        if (grabbed) {
            outputAngle = angle;
        }
        SetRotation();
        SetFill();
        TryGrab();
    }

}
