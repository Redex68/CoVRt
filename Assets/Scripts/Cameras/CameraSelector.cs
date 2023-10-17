using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraSelector : MonoBehaviour
{
    [SerializeField] List<GameObject> cameras;
    [SerializeField] float deadzone = 0.1f;
    /// <summary> The width of the cone in degrees </summary>
    [SerializeField] float coneWidth = 5;
    [SerializeField] GameObject arrow;
    [SerializeField] Vector2 arrowScaling;

    private CameraControler controler;
    private UnityAction onClick;
    private GameObject highlightedCamera;
    private GameObject selectedCamera;

    // Start is called before the first frame update
    void Start()
    {
        selectedCamera  = cameras[0];
        onClick += Select;
        controler = GetComponent<TestCameraControler>();
        controler.AddSelectListener(onClick);
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

    void Select()
    {
        if(highlightedCamera != null)
        {
            selectedCamera.GetComponent<Toggle>().isOn = false;
            selectedCamera = highlightedCamera;
            highlightedCamera = null;
        }
    }

    private GameObject ClosestCameraInDir(Vector2 dir)
    {
        GameObject currentCamera = selectedCamera;
        float closestDist = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject camera in cameras)
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
        if(camera == null)
        {
            highlightedCamera = null;
            selectedCamera.GetComponent<Toggle>().isOn = true;
            return;
        }
        
        if(highlightedCamera != null && selectedCamera != camera)
        {
            highlightedCamera.GetComponent<Toggle>().isOn = false;
        }

        camera.GetComponent<Toggle>().isOn = true;
        highlightedCamera = camera;
    }
}
