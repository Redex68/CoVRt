using UnityEngine;

public class ControlPanelServerMock: MonoBehaviour
{
    public GameEvent serverFound;
    public GameEvent serverDisconnected;    
    [SerializeField] int newFloor = 0;
    [SerializeField] int joystickX = 512;
    [SerializeField] int joystickY = 512;
    [SerializeField] int joystickDown = 0;
    [SerializeField] int dial1 = 512;
    [SerializeField] int dial2 = 512;
    [SerializeField] int buttonDown = 0;

    void Update()
    {
        ControlPanelServer.data = $"{newFloor}|{joystickX}|{joystickY}|{joystickDown}|{dial1}|{dial2}|{buttonDown}";
    }
}