using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Interface that represents the control panel joystick
public interface CameraControler
{
    //The direction in which e.g. the joystick is pointing in
    Vector2 GetDirection();
    //Add listener that will be called when the user atempts to select a camera (e.g. presses down on the joystick)
    void AddSelectListener(UnityAction action);
}
