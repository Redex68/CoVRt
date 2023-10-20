using UnityEngine;

public class ControlPanelServerMock: MonoBehaviour
{
    public GameEvent serverFound;
    public GameEvent serverDisconnected;    
    [SerializeField] bool floor = false;
    [SerializeField] [Range(0, 1023)] int joystickX = 512;
    [SerializeField] [Range(0, 1023)] int joystickY = 512;
    [SerializeField] bool joystickDown = false;
    [SerializeField] [Range(0, 1023)] int dial1 = 512;
    [SerializeField] [Range(0, 1023)] int dial2 = 512;
    [SerializeField] bool buttonDown = false;

    void Update()
    {
        ControlPanelServer.data = $"{(floor ? 1 : 0)}|{joystickX}|{joystickY}|{(joystickDown ? 1 : 0)}|{dial1}|{dial2}|{(buttonDown ? 1 : 0)}";
    }
}