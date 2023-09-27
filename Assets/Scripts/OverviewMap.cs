using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewMap : MonoBehaviour
{
    private int camIndex;
    private int layer;
    public Camera cam;
    public GameObject floor1Cams;
    public GameObject floor2Cams;
    
    // Start is called before the first frame update
    void Start()
    {
        layer = 6;
    }

    public void ToggleCamera(int step)
    {
        Debug.Log(step);
        // increment index by step, loop back around if below/above limits
        camIndex += step;
        if (camIndex < 0) camIndex = floor1Cams.transform.childCount + floor2Cams.transform.childCount - 1;
        else if (camIndex > floor1Cams.transform.childCount + floor2Cams.transform.childCount - 1) camIndex = 0;

        // manually switch camera by setting the toggle to on
        Toggle toggle;
        if (camIndex < floor1Cams.transform.childCount) toggle = floor1Cams.transform.GetChild(camIndex).GetComponent<Toggle>();
        else toggle = floor2Cams.transform.GetChild(camIndex - floor1Cams.transform.childCount).GetComponent<Toggle>();
        toggle.isOn = true;
    }

    public void SwitchFloor(int floor)
    {
        //if (floor >= 1 && floor <= 5) layer = floor + 5;
        //else layer = 6;
        //cam.cullingMask = (1 << layer) | (1 << LayerMask.NameToLayer("Stairs")) | (1 << LayerMask.NameToLayer("MapIcon"));

        // for floor 1
        if (floor == 1)
        {
            cam.nearClipPlane = 95;
            cam.farClipPlane = 105;

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
        else if (floor == 2)
        {
            cam.nearClipPlane = 85;
            cam.farClipPlane = 95;

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
    }
}
