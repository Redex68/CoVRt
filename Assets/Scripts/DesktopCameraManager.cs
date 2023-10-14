using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopCameraManager : MonoBehaviour
{

    // inputSystem embedded actions
    [SerializeField]
    private InputAction nextCameraAction;
    [SerializeField]
    private InputAction prevCameraAction;

    [SerializeField]
    [Tooltip("Set at runtime")]
    private CameraPoses cameraPoses = null;
    [SerializeField]
    [Tooltip("Set at runtime")]
    private OverviewMap overviewMap = null;

    [SerializeField]
    [Tooltip("Set at runtime")]

    private Camera mainDesktopCamera = null;

    [SerializeField]
    private string currentCamera = null;
    [SerializeField]
    private int currentCameraIndex = 0;

    void Start()
    {
        cameraPoses = FindObjectOfType<CameraPoses>(true);
        if (cameraPoses == null)
        {
            Debug.LogError("CameraPoses not found in DesktopCameraManager");
            return;
        }
        overviewMap = FindObjectOfType<OverviewMap>(true);
        if (overviewMap == null)
        {
            Debug.LogError("OverviewMap not found in DesktopCameraManager");
            return;
        }

        mainDesktopCamera = GameObject.FindGameObjectWithTag("MainDesktopCamera").GetComponent<Camera>();
        if (mainDesktopCamera == null)
        {
            Debug.LogError("MainDesktopCamera not found in DesktopCameraManager");
            return;
        }

        // set the current camera to the first camera
        currentCamera = cameraPoses.GetName(currentCameraIndex);

        // bind callbacks for input system
        nextCameraAction.performed += _ => { StepCameraForward(); };
        prevCameraAction.performed += _ => { StepCameraBackward(); };
    }

    void OnEnable()
    {
        nextCameraAction.Enable();
        prevCameraAction.Enable();
    }
    void OnDisable()
    {
        nextCameraAction.Disable();
        prevCameraAction.Disable();
    }


    public void StepCameraForward()
    {
        int total = cameraPoses.Count;
        SetCurrent((currentCameraIndex + 1) % total);
    }

    public void StepCameraBackward()
    {
        int total = cameraPoses.Count;

        int newIndex = (currentCameraIndex - 1) % total;
        if (newIndex < 0) currentCameraIndex += total;
        SetCurrent(newIndex);
    }

    public void SetCurrent(int index)
    {
        currentCameraIndex = index;
        currentCamera = cameraPoses.GetName(currentCameraIndex);
    }

    public void SetCurrent(string name)
    {
        currentCamera = name;
        currentCameraIndex = cameraPoses.GetIndex(currentCamera);
    }

    //public void MoveCamera(int index)
    //{
    //    SetCurrent(index);
    //}

    public string GetCurrentName()
    {
        return currentCamera;
    }

    public int GetCurrentIndex()
    {
        return currentCameraIndex;
    }

    // Update is called once per frame
    void Update()
    {
        Transform targetPose = cameraPoses.GetTransform(currentCamera);
        mainDesktopCamera.transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);
    }

}
