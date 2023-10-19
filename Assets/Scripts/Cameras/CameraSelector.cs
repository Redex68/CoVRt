using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraSelector : MonoBehaviour
{
    [SerializeField] float deadzone = 0.1f;
    /// <summary> The width of the cone in degrees </summary>
    [SerializeField] float coneWidth = 5;
    [SerializeField] GameObject arrow;
    [SerializeField] Vector2 arrowScaling;
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color highlightColor = Color.cyan;
    [SerializeField] float highlightBreatheSpeed = 1.0f;
    [SerializeField] float highlightBreatheSizeIncrease = 0.5f;

    private CameraControler controler;
    private UnityAction onClick;
    private GameObject highlightedCamera;
    private GameObject selectedCamera;
    private int currentFloor = 0;
    private List<List<GameObject>> floorCamIcons = new List<List<GameObject>>();
    private List<GameObject> lastSelectedCamIcon = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        onClick += Select;
        controler = GetComponent<TestCameraControler>();
        controler.AddSelectListener(onClick);

        OverviewMap overviewMap = GetComponent<OverviewMap>();
        floorCamIcons.Add(new List<GameObject>());
        floorCamIcons.Add(new List<GameObject>());

        foreach(Transform t in overviewMap.floor1Cams.transform)
            floorCamIcons[0].Add(t.gameObject);
        foreach(Transform t in overviewMap.floor2Cams.transform)
            floorCamIcons[1].Add(t.gameObject);
        for(int i = 0; i < floorCamIcons.Count; i++)
            lastSelectedCamIcon.Add(floorCamIcons[i][0]);

        selectedCamera = lastSelectedCamIcon[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = controler.GetDirection();
        if(dir.magnitude > deadzone)
        {
            arrow.SetActive(true);
            arrow.transform.position = selectedCamera.transform.position;
            arrow.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, dir));
            arrow.transform.localScale = new Vector3(dir.magnitude * (arrowScaling.x - 1.0f) + 1.0f, dir.magnitude * (arrowScaling.y - 1.0f) + 1.0f, 1.0f);
            GameObject closest = ClosestCameraInDir(dir);
            HighlightCamera(closest);
        }
        else
        {
            arrow.SetActive(false);
            HighlightCamera(null);
        }
    }

    //Called when the user presses the button to select a highlighted camera
    void Select()
    {
        if(highlightedCamera != null)
        {
            selectedCamera = highlightedCamera;
            HighlightCamera(null); 
            selectedCamera.GetComponent<Toggle>().isOn = true;

            lastSelectedCamIcon[currentFloor] = selectedCamera;
        }
    }

    //Called when the user switches floors
    public void FloorChanged(int floor)
    {
        currentFloor = floor;
        HighlightCamera(lastSelectedCamIcon[currentFloor]);
        Select();
    }

/// <summary> Returns the closest camera in the specified direction relative to the currently selected camera </summary>
    private GameObject ClosestCameraInDir(Vector2 dir)
    {
        GameObject currentCamera = selectedCamera;
        float closestDist = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject camera in floorCamIcons[currentFloor])
        {
            if(camera == currentCamera) continue;

            Vector2 relativeDir = currentCamera.transform.position - camera.transform.position;
            if(Vector2.Angle(-relativeDir, dir) < coneWidth && relativeDir.magnitude < closestDist)
            {
                closest = camera;
                closestDist = relativeDir.magnitude;
            }
        }

        return closest;
    }

    private void HighlightCamera(GameObject camera)
    {
        if(camera == selectedCamera) return;
        //Reset the color of the last highlighted camera when no camera is being highlighted
        if(camera == null)
        {
            if(highlightedCamera != null)
            {
                SetColorTo(highlightedCamera, defaultColor);
                highlightedCamera.transform.localScale = Vector3.one;
                highlightedCamera = null;
            }
            return;
        }
        
        if(highlightedCamera != null)
        {
            SetColorTo(highlightedCamera, defaultColor);
            highlightedCamera.transform.localScale = Vector3.one;
        }

        highlightedCamera = camera;
        SetColorTo(highlightedCamera, highlightColor);
        //Create the breathing effect
        float scale = Mathf.Abs(Mathf.InverseLerp(0, highlightBreatheSpeed, Time.time % highlightBreatheSpeed) - 0.5f) * highlightBreatheSizeIncrease + 1.0f + highlightBreatheSizeIncrease / 2;
        highlightedCamera.transform.localScale = new Vector3(scale, scale, 1.0f);
    }

    private void SetColorTo(GameObject camera, Color color)
    {
        Image image = camera.GetComponentInChildren<Image>();
        if(image != null) image.color = color;
    }
}
