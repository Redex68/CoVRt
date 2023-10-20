using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] public float rotationGoal;
    [SerializeField, Range(0, 360)] private float maxRotationAngle;
    [SerializeField] private FloatVariable cameraRotationSeped;
    [SerializeField] private Dial dial;

    private float currentRotation = 0.0f;
    private bool isSelected = false;
    private CameraPoses poses;

    // Start is called before the first frame update
    void Start()
    {
        poses = FindObjectOfType<CameraPoses>(true);
    }

    // Update is called once per frame
    void Update()
    {
        checkIsSelected();
        if(isSelected) UpdateGoal();
        UpdateRotation();
    }
    private void checkIsSelected()
    {
        if(DesktopCameraManager.currentCameraIndex == poses.namesToIndices[name])
        {
            //Set the dial's output angle to the correct one for the currently selected camera
            if(!isSelected)
            {
                dial.SetOutput(Mathf.InverseLerp(-maxRotationAngle / 2, maxRotationAngle / 2, rotationGoal));
            }
            isSelected = true;
        }
        else isSelected = false;
    }
    
    private void UpdateGoal()
    {
        if(dial.grabbed)
        {
            rotationGoal = Mathf.Lerp(-maxRotationAngle, maxRotationAngle, dial.GetOutput()) / 2;
        }
    }

    //This will get out of sync with time, but it would be a pain to make it work correctly
    private void UpdateRotation()
    {
        if(currentRotation != rotationGoal)
        {
            float toRotate = Mathf.Min(Mathf.Abs(currentRotation - rotationGoal), cameraRotationSeped.Value * Time.deltaTime);
            if(currentRotation > rotationGoal) toRotate = -toRotate;
            currentRotation += toRotate;
            transform.Rotate(Vector3.up, toRotate, Space.World);
        }
    }
}
