using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopCameraManager : MonoBehaviour
{

    // inputSystem embedded actions
    [SerializeField]
    private InputAction nextCamera;
    [SerializeField]
    private InputAction prevCamera;
    
    
    
    private Camera mainCamera;

    [SerializeField]
    private Transform[] cameraPoses;

    [SerializeField]
    private int currentCamera = 0;
    void Awake() {
        // get a reference to the camera
        mainCamera = GetComponentInChildren<Camera>();
        // bind callbacks for input system
        nextCamera.performed += _ => {StepCamera(1);};
        prevCamera.performed += _ => {StepCamera(-1);};
    }

    /*
    StepCamera - changes the current camera position to the previous or next position.
    @param step - 1 to move to the next camera, -1 to go to the previous camera.
    */
    public void StepCamera(int step) {
        currentCamera = (currentCamera + step) % cameraPoses.Length;
        // because % is not euclidean modulo in c# ;_;
        if (currentCamera < 0) currentCamera += cameraPoses.Length;
    }

    /// <summary>
    /// Moves the camera to the position at the provided index
    /// </summary>
    /// <param name="index">the index of the position to move camera to</param>
    public void MoveCamera(int index)
    {
        if (index >= 0 && index < cameraPoses.Length) currentCamera = index;
    }

    // Update is called once per frame
    void Update()
    {
        Transform targetPose = cameraPoses[currentCamera];
        mainCamera.transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);
    }

    void OnEnable() {
        nextCamera.Enable();
        prevCamera.Enable();
    }
    void OnDisable() {
        nextCamera.Disable();
        prevCamera.Disable();
    }
}
