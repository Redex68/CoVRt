using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewMap : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Set at runtime")]
    private DesktopCameraManager desktopCameraManager;
    private int mapCameraIndex;
    //private int layer;
    private Camera mapCamera;
    public GameObject floor1Cams;
    public GameObject floor2Cams;

    [SerializeField]
    private ToggleGroup floorSwitch = null;

    [SerializeField]
    private ToggleGroup cameraSwitch = null;

    private Toggle[] floorToggles = null;
    private Toggle[] cameraToggles = null;

    private Toggle selectedFloorToggle = null;

    private Toggle selectedCameraToggle = null;

    void Start()
    {
        desktopCameraManager = FindObjectOfType<DesktopCameraManager>(true);
        if (desktopCameraManager == null)
        {
            Debug.LogError("DesktopCameraManager not found in OverviewMap");
            return;
        }

        mapCamera = GameObject.Find("OverviewMapCamera").GetComponent<Camera>();
        if (mapCamera == null)
        {
            Debug.LogError("OverviewMapCamera not found in OverviewMap");
            return;
        }

        if (floorSwitch == null)
        {
            Debug.LogError("FloorSwitch not found in OverviewMap");
            return;
        }

        if (cameraSwitch == null)
        {
            Debug.LogError("CameraSwitch not found in OverviewMap");
            return;
        }

        floorToggles = floorSwitch.GetComponentsInChildren<Toggle>();
        if (floorToggles == null)
        {
            Debug.LogError("FloorSwitch has no Toggles");
            return;
        }

        foreach (Toggle t in floorToggles)
        {
            t.onValueChanged.AddListener((id) => SwitchFloor(t));
        }


        cameraToggles = cameraSwitch.GetComponentsInChildren<Toggle>();
        if (cameraToggles == null)
        {
            Debug.LogError("CameraSwitch has no Toggles");
            return;
        }

        foreach (Toggle t in cameraToggles)
        {
            t.onValueChanged.AddListener((id) => SwitchCamera(t));
        }

        SwitchFloor(floorToggles[0]);
    }


    public void SwitchCamera(Toggle t)
    {
        desktopCameraManager.SetCurrent(t.name);
        selectedCameraToggle = t;
    }

    public void SwitchFloor(Toggle t)
    {
        //if (floor >= 1 && floor <= 5) layer = floor + 5;
        //else layer = 6;
        //cam.cullingMask = (1 << layer) | (1 << LayerMask.NameToLayer("Stairs")) | (1 << LayerMask.NameToLayer("MapIcon"));
        // for floor 1
        if (t.name == "Floor1")
        {
            Debug.Log("Floor1");
            mapCamera.nearClipPlane = 95;
            mapCamera.farClipPlane = 105;

            selectedFloorToggle = t;
            foreach (Transform c in floor1Cams.transform)
            {
                c.GetComponent<Toggle>().interactable = true;
                c.transform.GetChild(0).gameObject.SetActive(true);
            }
            foreach (Transform c in floor2Cams.transform)
            {
                c.GetComponent<Toggle>().interactable = false;
                c.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        // for floor 2
        else if (t.name == "Floor2")
        {
            Debug.Log("Floor2"); // TODO: IS THIS NEEDED?
            mapCamera.nearClipPlane = 85;
            mapCamera.farClipPlane = 95;

            selectedFloorToggle = t;

            foreach (Transform c in floor1Cams.transform)
            {
                c.GetComponent<Toggle>().interactable = false;
                c.transform.GetChild(0).gameObject.SetActive(false);
            }
            foreach (Transform c in floor2Cams.transform)
            {
                c.GetComponent<Toggle>().interactable = true;
                c.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("Invalid floor name");
        }
    }
}
