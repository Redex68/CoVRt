using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalCameraControler : MonoBehaviour, CameraControler
{

    public void AddSelectListener(UnityAction action)
    {
        ControlPanel.Instance.AddCameraSelectListener(action);
    }

    public Vector2 GetDirection()
    {
        return ControlPanel.Instance.GetJoystickDirection();
    }
}
