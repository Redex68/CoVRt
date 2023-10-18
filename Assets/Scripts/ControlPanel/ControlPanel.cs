using UnityEngine;
using UnityEngine.Events;

//Interface that represents the control panel joystick
public abstract class ControlPanel : MonoBehaviour
{
    public ControlPanel Instance;


    /// <summary> The direction in which the joystick is pointing in </summary>
    /// <returns>
    ///     Vector2 with values [-1.0, 1.0] for each axis
    /// </returns>
    public abstract Vector2 GetJoystickDirection();
    //Add listener that will be called when the user atempts to select a camera (e.g. presses down on the joystick)
    public abstract void AddCameraSelectListener(UnityAction action);

    /// <summary>
    ///     Adds a listener that will be called when the user switches floors.
    ///     If floor 1 is being selected, 0 is passed to the listener. If floor 2
    ///     is being selected, 1 is passed to the listener.
    /// </summary>
    public abstract void AddFloorSwitchListener(UnityAction<int> action);

    /// <summary> The position of dial 1 <summary>
    /// <returns> A float with a value [0.0, 1.0] </returns>
    public abstract float GetDial1Position();
    /// <summary> The position of dial 2 <summary>
    /// <returns> A float with a value [0.0, 1.0] </returns>
    public abstract float GetDial2Position();
}
