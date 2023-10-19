using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;
using UnityEditor.Experimental.GraphView;

public class ControlPanelPhysical : ControlPanel
{
    [SerializeField] bool invertJoystickX;
    [SerializeField] bool invertJoystickY;
    [SerializeField] bool invertDial1;
    [SerializeField] bool invertDial2;

    private UnityEvent cameraSelect = new();
    private UnityEvent<int> floorSwitch = new();

    private int floor = 0;
    private Vector2 joystick = Vector2.zero;
    private bool buttonWasHeldDown = false;
    private List<float> dialPositions = new List<float>();

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialPositions.Add(0);
        dialPositions.Add(0);
        enabled = false;
    }

    void Update()
    {
        ReadData();
    }

    //Data format:
    //f|xxxx|yyyy|jb|dddd|eeee|b
    //f - floor (0 or 1)
    //xxxx - joystick x axis [0, 1023]
    //yyyy - joystick y axis [0, 1023]
    //jb - joystick button (0 or 1)
    //dddd - dial 1 [0, 1023]
    //eeee - dial 2 [0, 1023]
    //b - button button (0 or 1)
    private void ReadData()
    {
        string[] segments = ControlPanelServer.data.Split('|');
        if(segments.Length != 7) return;

        int newFloor = int.Parse(segments[0]);
        int _joystickX = int.Parse(segments[1]);
        int _joystickY = int.Parse(segments[2]);
        bool joystickDown = int.Parse(segments[3]) == 1;
        int _dial1 = int.Parse(segments[4]);
        int _dial2 = int.Parse(segments[5]);
        bool buttonDown = int.Parse(segments[6]) == 1;

        joystick = new Vector2((invertJoystickX ? -1 : 1) * (_joystickX - 512) / 512.0f, (invertJoystickY ? -1 : 1) * (_joystickY - 512) / 512.0f);
        dialPositions[0] = _dial1 / 1023.0f;
        if(invertDial1) dialPositions[0] = 1.0f - dialPositions[0];
        dialPositions[1] = _dial2 / 1023.0f;
        if(invertDial2) dialPositions[1] = 1.0f - dialPositions[1];

        if(newFloor != floor)
        {
            Debug.Log("Invoking floor switch");
            floorSwitch.Invoke(newFloor);
            floor = newFloor;
        }
        if(buttonDown && !buttonWasHeldDown)
        {
            cameraSelect.Invoke();
            buttonWasHeldDown = true;
        }
        else if(!buttonDown)
        {
            buttonWasHeldDown = false;
        }
    }


    public override void AddCameraSelectListener(UnityAction action)
    {
        cameraSelect.AddListener(action);
    }

    public override void AddFloorSwitchListener(UnityAction<int> action)
    {
        floorSwitch.AddListener(action);
    }

    public override List<float> GetDialPositions()
    {
        return dialPositions;
    }

    public override Vector2 GetJoystickDirection()
    {
        return joystick;
    }
}
